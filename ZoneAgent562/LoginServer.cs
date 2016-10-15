using pConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZoneAgent562
{
    internal class LSuserInfo
    {
        internal string Acc { get; private set; }
        internal DateTime Time { get; private set; }

        internal LSuserInfo(string account)
        {
            this.Acc = account;
            this.Time = DateTime.Now;
        }
    }

    internal class LoginServer : IDisposable
    {
        private FrmMain _Main;
        private Timer LS_Reporter;
        private Timer Prepared_Checker;
        internal static EventDrivenTCPClient LS;
        internal static Dictionary<uint, LSuserInfo> PreparedAcc;

        public void Dispose()
        {
            this.LS_Reporter.Dispose();
            this.Prepared_Checker.Dispose();
        }

        internal LoginServer(FrmMain frm)
        {
            _Main = frm;
            LS_Reporter = new Timer(LS_Report_Tick, null, Timeout.Infinite, Timeout.Infinite);
            Prepared_Checker = new Timer(Prepared_Checker_Tick, null, Timeout.Infinite, Timeout.Infinite);
            LS = new EventDrivenTCPClient(Config.LS.IP, Config.LS.Port);
            LS.ConnectionStatusChanged += LS_ConnectionStatusChanged;
            LS.DataReceived += LS_DataReceived;
            PreparedAcc = new Dictionary<uint, LSuserInfo>();
            try
            {
                LS.Connect();
            }
            catch { }
        }
        /// <summary>
        /// 500ms마다 반복하면서 Prepared리스트에서 로그인 대기 시간이 초과된 목록을 삭제.
        /// </summary>
        /// <param name="state"></param>
        private void Prepared_Checker_Tick(Object state)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pUser in PreparedAcc.Where(x => x.Value.Time.AddMilliseconds(1400) < DateTime.Now).ToList())
            {
                sb.Clear();
                _Main.UpdateLogMsg(sb.AppendFormat("Prepared.Remove: {0} {1} Time out", pUser.Key, pUser.Value.Acc).ToString());
                PreparedAcc.Remove(pUser.Key);
            }
        }

        /// <summary>
        /// 5초마다 LS에 보내는 생존 패킷
        /// </summary>
        /// <param name="state"></param>
        private void LS_Report_Tick(Object state)
        {
            MSG_ZA2LS_REPORT LS_Report = new MSG_ZA2LS_REPORT();
            LS_Report.dwPlayerCount = (uint)ZoneAgent._Players.Count;
            LS_Report.byZSCount1 = LS_Report.byZSCount2 = (byte)Config.ZSList.Count;
            LS.Send(LS_Report.Serialize());
            //_Main.UpdateLogMsg("Report:" + BitConverter.ToString(LS_Report.Serialize()).Replace("-", " "));
        }
        /// <summary>
        /// LS의 연결 상태 변동에 따른 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="status"></param>
        private void LS_ConnectionStatusChanged(EventDrivenTCPClient sender, EventDrivenTCPClient.ConnectionStatus status)
        {
            if (status == EventDrivenTCPClient.ConnectionStatus.Connected)
            {
                _Main.UpdateLSinfo("Connected");
                MSG_ZA2LS_CONNECT LS_Connect = new MSG_ZA2LS_CONNECT();
                LS_Connect.byServerID = Config.ZA.sID;
                LS_Connect.byAgentID = Config.ZA.aID;
                LS_Connect.szZAIP = Config.ZA.IP.ToString();
                LS_Connect.dwZAPort = (uint)Config.ZA.Port;
                LS.Send(LS_Connect.Serialize());

                //LS가 종료되었다 재실행 되었을 경우를 대비하여 현재 접속 유저 정보를 LS에 알려준다.
                if (ZoneAgent._Players != null)
                {
                    foreach (var player in ZoneAgent._Players)
                    {
                        MSG_ZA2LS_LOGIN_USER_LIST LS_ConnectedPlayer = new MSG_ZA2LS_LOGIN_USER_LIST();
                        LS_ConnectedPlayer.MsgHeader.dwPCID = player.Key;
                        LS_ConnectedPlayer.szUserAccount = player.Value.Account;
                        LS_ConnectedPlayer.szUserIP = player.Value.IPadress;
                        LS.Send(LS_ConnectedPlayer.Serialize());
                    }
                }
                LS_Reporter.Change(0, 5000);
                Prepared_Checker.Change(0, 500);
            }
            else
            {
                _Main.UpdateLSinfo("Disconnected");
                LS_Reporter.Change(Timeout.Infinite, Timeout.Infinite);
                Prepared_Checker.Change(Timeout.Infinite, Timeout.Infinite);
                PreparedAcc.Clear();
            }
        }
        /// <summary>
        /// LS에서 들어오는 패킷 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void LS_DataReceived(EventDrivenTCPClient sender, object data)
        {
            try
            {
                byte[] packet = (byte[])Convert.ChangeType(data, typeof(byte[]));
                MSG_HEAD_WITH_PROTOCOL pHeader = new MSG_HEAD_WITH_PROTOCOL();
                pHeader.Deserialize(ref packet);
                switch (pHeader.byCtrl)
                {
                    case 0x01:
                        switch (pHeader.byCmd)
                        {
                            case 0xE1: //LS가 보내주는 접속할 새 클라이언트 정보 : Uid, 0A:userid(30)
                                if (!PreparedAcc.ContainsKey(pHeader.dwPCID))
                                {
                                    MSG_LS2ZA_ACC_LOGIN accPrepare = new MSG_LS2ZA_ACC_LOGIN();
                                    accPrepare.Deserialize(ref packet);
                                    PreparedAcc.Add(pHeader.dwPCID, new LSuserInfo(accPrepare.szAccount));
                                    //zonelog update
                                    _Main.UpdateLogMsg(string.Format("<LC>UID={0} {1} Prepared", pHeader.dwPCID, accPrepare.szAccount));
                                }
                                break;
                            case 0xE3: //duplicate login; request DC to ZA from loginserver
                                if (ZoneAgent._Players.ContainsKey(pHeader.dwPCID))
                                {
                                    MSG_LS2ZA_REQ_LOGOUT reqLogout = new MSG_LS2ZA_REQ_LOGOUT();
                                    reqLogout.Deserialize(ref packet);
                                    //zonelog update
                                    _Main.UpdateLogMsg(string.Format("<LC>UID={0} {1} Dropped, Duplicate login", reqLogout.MsgHeader.dwPCID, reqLogout.szAccount));
                                    ZoneAgent.RemoveClient(pHeader.dwPCID, reqLogout.byReason);
                                }
                                break;
                            default: //any other packet received
                                Logger.NewPacket("LS->ZA", packet, sender.IP.ToString(), ClientVer.undefined);
                                break;
                        }
                        break;
                    default:
                        Logger.NewPacket("LS->ZA", packet, sender.IP.ToString(), ClientVer.undefined);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("LS_DataReceived: {0}", ex));
            }
        }
    }
}
