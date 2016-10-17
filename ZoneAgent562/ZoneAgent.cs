using pConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ZoneAgent562
{
    internal class ZoneAgent : IDisposable
    {
        private FrmMain _Main;
        private TcpListener ZA;
        internal static Dictionary<uint, Client> _Players;
        internal static Timer ReqTick_Sender;
        internal static Timer dcClient_Checker;
        //internal static Timer Notice_Sender;
        internal static string szNotice;
        private int MaxPlayerCount;

        public void Dispose()
        {
            ReqTick_Sender.Dispose();
            dcClient_Checker.Dispose();
        }
        internal ZoneAgent(FrmMain frm)
        {
            MaxPlayerCount = 0;
            szNotice = string.Empty;
            _Main = frm;
            _Players = new Dictionary<uint, Client>();
            ZA = new TcpListener(IPAddress.Any, Config.ZA.Port);
            dcClient_Checker = new Timer(RemoveDisconnectedClient, null, 5000, 1000);
            ReqTick_Sender = new Timer(ReqTimeTick_Tick, null, 5000, 5000);
            //Notice_Sender = new Timer(PushNotice_Tick, null, 5000, 10000);

            try
            {
                ZA.Start();
                ZA.BeginAcceptTcpClient(ClientHandler, null);
                Logger.Write("Start => ZoneAgent started listening");
            }
            catch { }
        }

        private void ClientHandler(IAsyncResult asyncResult)
        {
            try
            {
                TcpClient client = ZA.EndAcceptTcpClient(asyncResult);
                byte[] buffer = new byte[client.ReceiveBufferSize];
                Client newClient = new Client(client, buffer);
                NetworkStream networkStream = newClient.NetworkStream;
                networkStream.BeginRead(newClient.Buffer, 0, newClient.Buffer.Length, OnDataRead, newClient);
                ZA.BeginAcceptTcpClient(ClientHandler, null);
            }
            catch { }
        }

        private void OnDataRead(IAsyncResult asyncResult)
        {
            Client client = asyncResult.AsyncState as Client;
            try
            {
                if (client == null) return;
                NetworkStream networkStream = client.NetworkStream;
                IPEndPoint newClientEp = (IPEndPoint)client.TcpClient.Client.RemoteEndPoint;
                int readSize = networkStream.EndRead(asyncResult);
                if (readSize < 10)
                {
                    client.TcpClient.Client.Dispose();
                    return;
                }
                //클라가 보내는 처음 패킷(로그인 요청) 사이즈로 클라 버전 확인
                if (client.Ver == ClientVer.undefined)
                {
                    client.Ver = (ClientVer)readSize;
                }

                List<byte[]> packetList;
                Packet.SplitPackets(client.Buffer, readSize, out packetList);
                foreach (byte[] sPacket in packetList)
                {
                    byte[] packet = sPacket;
                    MSG_HEAD_WITH_PROTOCOL pHeader = new MSG_HEAD_WITH_PROTOCOL();
                    pHeader.Deserialize(ref packet);
                    switch (pHeader.byCtrl)
                    {
                        case 0x01:
                            switch (pHeader.byCmd)
                            {
                                case 0xE2: // Account Login
                                    MSG_CL2ZA_LOGIN nPlayer = new MSG_CL2ZA_LOGIN();
                                    nPlayer.Deserialize(ref packet);
                                    if (pHeader.dwPCID == nPlayer.dwPCID)
                                    {
                                        //유저 로그인은 새 쓰레드에서 처리
                                        new Thread(() => isPreparedUser(ref client, nPlayer.dwPCID, nPlayer.szAccount, newClientEp.Address.ToString())).Start();
                                    }
                                    else
                                    {
                                        client.TcpClient.Client.Dispose();
                                    }
                                    break;
                                case 0xF0: //TimeTick - Check Speed Hack
                                    if (_Players.ContainsKey(client.Uid))
                                    {
                                        new Thread(() => CheckTimeTick(packet, ref client)).Start();
                                    }
                                    else
                                    {
                                        client.TcpClient.Client.Dispose();
                                    }
                                    break;
                                default:
                                    Logger.NewPacket("CL->ZA", packet, client.IPadress, client.Ver);
                                    client.TcpClient.Client.Dispose();
                                    break;
                            }
                            break;
                        case 0x03:
                            if (_Players.ContainsKey(client.Uid))
                            {
                                //Convert: add uid, decrypt, if v578 -> v219(v562) type
                                Config.mConvert.Convert_C2S(ref packet, client.Uid, client.Ver, client.IPadress);
                                pHeader.Deserialize(ref packet);

                                switch (pHeader.wProtocol)
                                {
                                    case 0x1106: //Character login
                                    case 0x2322: //Transfer Clan Mark
                                    case 0x2323: //Clan...
                                    case 0xA001: //Create Character
                                    case 0xA002: //Delete Character
                                        //보내기전 패킷 암호화, ZS에 보내는 패킷은 562버전으로
                                        Config.mConvert.Crypter.Encrypt(ref packet, ClientVer.v562);
                                        ZoneServer.ZS[Config.AS_ID].Send(packet);
                                        break;
                                    case 0x1112: //Teleport packet
                                        new Thread(() => isPossibleTeleport(packet, ref client)).Start();
                                        break;
                                    case 0x1108: //logout
                                        //AS에 있을때는 cl->za->as : as에서 답 없음
                                        //ZS에 있을때는 1108답장 있음 ->cl
                                        client.Reason = 0;
                                        Config.mConvert.Crypter.Encrypt(ref packet, ClientVer.v562);
                                        ZoneServer.ZS[client.ZoneStatus].Send(packet);
                                        break;
                                    default:
                                        //보내기전 패킷 암호화, ZS에 보내는 패킷은 562버전으로
                                        Config.mConvert.Crypter.Encrypt(ref packet, ClientVer.v562);
                                        ZoneServer.ZS[client.ZoneStatus].Send(packet);
                                        break;
                                }
                            }
                            else
                            {
                                client.TcpClient.Client.Dispose();
                            }
                            break;
                        default:
                            Logger.NewPacket("CL->ZA", packet, client.IPadress, client.Ver);
                            client.TcpClient.Client.Dispose();
                            break;
                    }
                }
                networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, OnDataRead, client);
            }
            catch (Exception ex)
            {
#if DEBUG
                Logger.Write(string.Format("OnDataRead: {0}", ex));
#endif
            }
        }

        /// <summary>
        /// 10초마다 상단 Notice 갱신
        /// </summary>
        /// <param name="state"></param>
        private void PushNotice_Tick(Object state)
        {
            foreach (var player in _Players.Where(x => x.Value.ZoneStatus != Config.AS_ID).ToList())
            {
                Client client = player.Value;
                string msg = string.Format("{0} Players[{1}]", szNotice, _Players.Count);
                byte[] pSayByte = Packet.SayMsg(client.Uid, msg, pSayType.Notice);
                //Config.mConvert.Convert_S2C(ref pSayByte, client.Ver);
                Write(ref client, pSayByte);
            }
        }

        /// <summary>
        /// 5초마다 모든 클라이언트에 Tick요청 패킷을 보낸다. 스피드 핵 탐지에 사용.
        /// </summary>
        private void ReqTimeTick_Tick(Object state)
        {
            foreach (var player in _Players.Where(x => x.Value.ZoneStatus != Config.AS_ID).ToList())
            {
                Client client = player.Value;
                client.TickSvr = (uint)(int.MaxValue & (Environment.TickCount - client.TickCltUnique));
                MSG_ZACL_CHK_TIMETICK reqTick = new MSG_ZACL_CHK_TIMETICK();
                reqTick.MsgHeader.dwPCID = client.Uid;
                reqTick.dwTickCount = client.TickCount++;
                reqTick.dwTickSvr = client.TickSvr;
                Write(ref client, reqTick.Serialize());
            }
        }

        private void CheckTimeTick(byte[] buffer, ref Client client)
        {
            uint TickSvrNow = (uint)(int.MaxValue & (Environment.TickCount - client.TickCltUnique));
            MSG_ZACL_CHK_TIMETICK cTick = new MSG_ZACL_CHK_TIMETICK();
            cTick.Deserialize(ref buffer);

            if (client.TickCltPre == 0)
            {
                //client.TickSvrPre = cTick.dwTickSvr;
                client.TickSvrPre = TickSvrNow;
                client.TickCltPre = cTick.dwTickClt;
                client.TickCountPre = cTick.dwTickCount;
                return;
            }
            if (cTick.dwTickCount - client.TickCountPre != 1)
            {
                Logger.WriteSDB(string.Format("[Index]  {0}  {1}  {2}  Pre:{3}  now:{4}  SendTime:{5}  {6}", client.Account, client.Character, client.IPadress, client.TickCountPre, cTick.dwTickCount, cTick.dwTickClt - client.TickCltPre, TickSvrNow - client.TickSvr));
                client.TickCountPre = cTick.dwTickCount++;
                client.TickCount = cTick.dwTickCount;
                return;
            }
            if (cTick.dwTickSvr != client.TickSvr)
            {
                Logger.WriteSDB(string.Format("[Hack]  {0}  {1}  {2}  Changed SvrTick:{3}->{4}  {5}", client.Account, client.Character, client.IPadress, client.TickSvr, cTick.dwTickSvr, TickSvrNow - client.TickSvr));
                RemoveClient(client.Uid, 4);
                return;
            }
            long TickSvrDiff = Math.Abs(TickSvrNow - client.TickSvrPre);
            long TickCltDiff = Math.Abs(cTick.dwTickClt - client.TickCltPre);
            if (Math.Abs(TickSvrDiff - TickCltDiff) > 500)
            {
                /*
                Logger.WriteSDB(string.Format("[Hack]  {0}  {1}  {2}  {3}  {4}  {5}", client.Account, client.Character, client.IPadress, TickSvrDiff, TickCltDiff, TickSvrNow - client.TickSvr));
                RemoveClient(client.Uid, 4);
                return;
                 * */
                if (++client.TickErrCount > 1)
                {
                    Logger.WriteSDB(string.Format("[Hack]  {0}  {1}  {2}  {3}  {4}  {5}", client.Account, client.Character, client.IPadress, TickSvrDiff, TickCltDiff, TickSvrNow - client.TickSvr));
                    RemoveClient(client.Uid, 4);
                    return;
                }
                client.TickCountPre = cTick.dwTickCount;
                client.TickCltPre = cTick.dwTickClt;
                client.TickSvrPre = TickSvrNow;
                return;
            }
            client.TickCountPre = cTick.dwTickCount;
            client.TickCltPre = cTick.dwTickClt;
            client.TickSvrPre = TickSvrNow;
            //client.TickSvrPre = cTick.dwTickSvr;
            client.TickErrCount = 0;
        }

        /// <summary>
        /// 텔레포트 요청시 유효한 요청인지 확인 & 사용자 레벨에 따른 맵 이동 제한
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="client"></param>
        private void isPossibleTeleport(byte[] buffer, ref Client client)
        {
            MSG_C2S_REQ_TELEPORT reqTel = new MSG_C2S_REQ_TELEPORT();
            reqTel.Deserialize(ref buffer);
            if (reqTel.wTargetLine > Config.TeleportList.Count)
            {
                //bad request. if approval, ZS is crash.
                MSG_S2C_SAY pSay = new MSG_S2C_SAY();
                pSay.MsgHeader.dwPCID = client.Uid;
                pSay.szWords = ByteTools.bSubString("Invalid request", 0x40);
                byte[] pSayByte = pSay.Serialize();
                //Config.mConvert.Convert_S2C(ref pSayByte, client.Ver);
                Write(ref client, pSayByte);
                Logger.WriteSDB(string.Format("[Try Crash]  {0}  {1}  {2}  {3}/{4}", client.Account, client.Character, client.IPadress, reqTel.wTargetLine, Config.TeleportList.Count));
                //client.TcpClient.Client.Disconnect(false);
            }
            else if (client.Level < Config.TeleportList[reqTel.wTargetLine].MapLv)
            {
                //account level is low
                MSG_S2C_SAY pSay = new MSG_S2C_SAY();
                pSay.MsgHeader.dwPCID = client.Uid;
                pSay.szWords = ByteTools.bSubString(string.Format("{0}, You can not use this map at your level", client.Account), 0x40);
                byte[] pSayByte = pSay.Serialize();
                //Config.mConvert.Convert_S2C(ref pSayByte, client.Ver);
                Write(ref client, pSayByte);
            }
            else
            {
                Config.mConvert.Crypter.Encrypt(ref buffer, ClientVer.v562);
                ZoneServer.ZS[client.ZoneStatus].Send(buffer);
            }
        }

        /// LS에서 작성하는 PreparedAcc 목록에 해당 유저가 있을경우에만 로그인 처리
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uid"></param>
        /// <param name="account"></param>
        /// <param name="ip"></param>
        private void isPreparedUser(ref Client client, uint uid, string account, string ip)
        {
            DateTime startTime = DateTime.Now;
            bool prepared = false;
            while (startTime.AddMilliseconds(1100) > DateTime.Now)
            {
                //LS에서 작성하는 PreparedAcc목록에 해당 유저가 있다면 정상 사용자로 등록
                if (LoginServer.PreparedAcc.ContainsKey(uid) && LoginServer.PreparedAcc[uid].Acc == account)
                {
                    client.Uid = uid;
                    client.IPadress = ip;
                    client.Account = account;
                    //유저마다 다른 틱을 사용하기 위해 uid를 이용해서 클라이언트마다 설정
                    client.TickCltUnique = Environment.TickCount << (byte)uid;
                    //LockedMap.ini 해당 유저의 레벨이 정의 되어 있으면 레벨 설정, 기본값은 0
                    if (Config.AccLevel.ContainsKey(account))
                        client.Level = Config.AccLevel[account];
                    _Players.Add(uid, client);
                    LoginServer.PreparedAcc.Remove(uid);
                    prepared = true;
                    break;
                }
            }

            if (prepared)
            {
                //로그인 서버에 유저 로그인 알리기
                MSG_ZA2LS_PREPARED_ACC_LOGIN pUserLogin = new MSG_ZA2LS_PREPARED_ACC_LOGIN();
                pUserLogin.MsgHeader.dwPCID = client.Uid;
                pUserLogin.szAccount = client.Account;
                LoginServer.LS.Send(pUserLogin.Serialize());

                //AS에 새로운 유저 로그인을 알림 -> 답으로 캐릭터 리스트(ACL) 패킷을 받는다
                MSG_ZA2AS_NEW_CLIENT reqCharList = new MSG_ZA2AS_NEW_CLIENT();
                reqCharList.MsgHeader.dwPCID = client.Uid;
                reqCharList.szAccount = client.Account;
                reqCharList.szClientIP = client.IPadress;
                ZoneServer.ZS[Config.AS_ID].Send(reqCharList.Serialize());

                MaxPlayerCount = _Players.Count > MaxPlayerCount ? _Players.Count : MaxPlayerCount;
                //player count update
                _Main.UpdateConnectionCount(_Players.Count, MaxPlayerCount);
                //zonelog update
                _Main.UpdateLogMsg(string.Format("{0} {1} User Joined", account, ip));
            }
            else
            {
                //시간내 인증이 이뤄지지 않으면 클라이언트 접속 끊기
                client.TcpClient.Client.Dispose();
            }
        }

        /// <summary>
        /// 클라이언트(플레이어)를 리스트에서 제거, zs에 접속중일때는 해당 zs에 dc패킷까지 보냄
        /// </summary>
        /// <param name="uid"></param>
        internal static void RemoveClient(uint uid, byte reason)
        {
            Client client = _Players[uid];
            MSG_ZA2ZS_ACC_LOGOUT pLogout = new MSG_ZA2ZS_ACC_LOGOUT();
            pLogout.MsgHeader.dwPCID = uid;
            pLogout.byReason = reason;
            ZoneServer.ZS[client.ZoneStatus].Send(pLogout.Serialize());
            client.Reason = reason;
            client.TcpClient.Client.Disconnect(false);
            //소켓 연결만 끊어주면 플레이어 리스트 에서는 체크 타이머에서 자동 제거 된다.
        }
        /// <summary>
        /// 소켓 연결상태 확인
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool isConnected(Socket s)
        {
            try { return !((s.Poll(1, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected); }
            catch { return false; }
        }
        /// <summary>
        /// 1초마다 클라이언트의 연결상태를 체크해서 현결 해제된 클라이언트는 제거한다.
        /// </summary>
        /// <param name="state"></param>
        private void RemoveDisconnectedClient(Object state)
        {
            foreach (var client in _Players.Where(x => !isConnected(x.Value.TcpClient.Client)).ToList())
            {
                //client.Reason이 기본값(1)그대로인데 소켓 연결이 끊어진 경우는 정상 로그아웃이 아니므로 ZS에도 로그아웃 패킷을 보낸다.
                if (client.Value.Reason == 1)
                {
                    MSG_ZA2ZS_ACC_LOGOUT pDisconnect = new MSG_ZA2ZS_ACC_LOGOUT();
                    pDisconnect.MsgHeader.dwPCID = client.Value.Uid;
                    pDisconnect.byReason = client.Value.Reason;
                    ZoneServer.ZS[client.Value.ZoneStatus].Send(pDisconnect.Serialize());
                }
                MSG_ZA2LS_ACC_LOGOUT pLogout = new MSG_ZA2LS_ACC_LOGOUT();
                pLogout.MsgHeader.dwPCID = client.Value.Uid;
                pLogout.byReason = client.Value.Reason;
                pLogout.szAccount = client.Value.Account;
                LoginServer.LS.Send(pLogout.Serialize());

                //zonelog update
                _Main.UpdateLogMsg(string.Format("{0} {1} DisconnectedUser.remove", client.Value.Account, client.Value.Uid));
                _Players.Remove(client.Key);
                //player count update
                _Main.UpdateConnectionCount(_Players.Count, MaxPlayerCount);
            }
        }

        internal static void Write(ref Client client, byte[] bytes)
        {
            try
            {
                Config.mConvert.Convert_S2C(ref bytes, client.Ver);
                client.TcpClient.GetStream().BeginWrite(bytes, 0, bytes.Length, WriteCallback, client.TcpClient);
            }
            catch { }
        }

        private static void WriteCallback(IAsyncResult result)
        {
            try
            {
                var tcpClient = result.AsyncState as TcpClient;
                if (tcpClient != null)
                {
                    NetworkStream networkStream = tcpClient.GetStream();
                    networkStream.EndWrite(result);
                }
            }
            catch { }
        }
    }
}
