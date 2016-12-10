using pConverter;
using System;
using System.Collections.Generic;

namespace ZoneAgent562
{
    internal class ZoneServer
    {
        private FrmMain _Main;
        internal static Dictionary<int, EventDrivenTCPClient> ZS;

        internal ZoneServer(FrmMain frm)
        {
            _Main = frm;
            ZS = new Dictionary<int, EventDrivenTCPClient>();
            //Zone Server Initialize
            foreach (var zs in Config.ZSList)
            {
                ZS.Add(zs.Value.aID, new EventDrivenTCPClient(zs.Value.IP, zs.Value.Port, zs.Value.aID));
                ZS[zs.Value.aID].DataReceived += ZS_DataReceived;
                ZS[zs.Value.aID].ConnectionStatusChanged += ZS_ConnectionStatusChanged;
                //Zone Server Start
                ZS[zs.Value.aID].Connect();
                Delay(300);
            }
        }

        /// <summary>
        /// Delay 함수 MS
        /// </summary>
        /// <param name="MS">(단위 : MS)
        ///
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        private void ZS_ConnectionStatusChanged(EventDrivenTCPClient sender, EventDrivenTCPClient.ConnectionStatus status)
        {
            if (status == EventDrivenTCPClient.ConnectionStatus.Connected)
            {
                MSG_ZA2ZS_CONNECT ZS_Connect = new MSG_ZA2ZS_CONNECT();
                ZS_Connect.byAgentID = Config.ZA.aID;
                ZS[sender.ID].Send(ZS_Connect.Serialize());
                Config.ZSList[sender.ID].Status = "Connected";
            }
            else
            {
                Config.ZSList[sender.ID].Status = "Disconnected";
            }
            _Main.UpdateConnectedZs();
        }

        private void ZS_DataReceived(EventDrivenTCPClient sender, object data)
        {
            try
            {
                byte[] buffer = (byte[])Convert.ChangeType(data, typeof(byte[]));
                List<byte[]> packetList;
                Packet.SplitPackets(buffer, buffer.Length, out packetList);
                foreach (byte[] sPacket in packetList)
                {
                    byte[] packet = sPacket;
                    Config.mConvert.Crypter.Decrypt(ref packet, ClientVer.v562);
                    MSG_HEAD_WITH_PROTOCOL pHeader = new MSG_HEAD_WITH_PROTOCOL();
                    pHeader.Deserialize(ref packet);
                    if (ZoneAgent._Players.ContainsKey(pHeader.dwPCID))
                    {
                        Client client = ZoneAgent._Players[pHeader.dwPCID];

                        if (pHeader.byCtrl == 0x01 && pHeader.byCmd == 0xE1)
                        {
                            //Zone Change Packet
                            _Main.UpdateLogMsg(string.Format("{0} {1} user zone changed {2}->{3}", client.Account, client.Uid, client.ZoneStatus, packet[0x0A]));
                            client.ZoneStatus = packet[0x0A];
                            continue;
                        }
                        else if (pHeader.wProtocol == 0x1800)
                        {
                            MSG_S2C_SAY pSay = new MSG_S2C_SAY();
                            pSay.Deserialize(ref packet);
                            if (pSayType.Notice == (pSayType)pSay.bySayType && "NOTICE" == pSay.szSayPC && uint.MaxValue == pSay.dwSayPCID)
                                ZoneAgent.szNotice = pSay.szWords;
                        }
                        else if (pHeader.wProtocol == 0x1107)
                        {
                            //world login: save character name and town
                            MSG_S2C_WORLD_LOGIN wLogin = new MSG_S2C_WORLD_LOGIN();
                            wLogin.Deserialize(ref packet);
                            client.Character = wLogin.szPCName;
                            client.Town = wLogin.byTown;
                        }
                        //encrypt후 클라이언트로 send
                        //Config.mConvert.Convert_S2C(ref packet, client.Ver);
#if DEBUG
                        PacketLogger.LogPacket(packet, "server_to_client", client.Character);
#endif
                        ZoneAgent.Write(ref client, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("ZS_DataReceived: {0}", ex));
            }
        }
    }
}
