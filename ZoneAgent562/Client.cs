using pConverter;
using System;
using System.Net.Sockets;

namespace ZoneAgent562
{
    internal class Client
    {
        internal Client(TcpClient tcpClient, byte[] buffer)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            TcpClient = tcpClient;
            Buffer = buffer;
            Level = 0;
            TickCltUnique = 0;
            Uid = 0;
            ZoneStatus = Config.AS_ID;
            Reason = 1; //0:logout, 1:connection lost, 2:duplicate login, 3:?, 4:speed hack
            TickCount = 0;
            TickSvr = 0;
            TickSvrPre = 0;
            TickCltPre = 0;
            TickCountPre = 0;
            TickErrCount = 0;
            Ver = ClientVer.undefined;
            Pkt219_Count = 0;
            Character = string.Empty;
        }

        internal TcpClient TcpClient { get; private set; }
        internal byte[] Buffer { get; private set; }
        internal NetworkStream NetworkStream
        {
            get { return TcpClient.GetStream(); }
        }
        //219에서만 사용하는 패킷 카운트, CL로 보내는 패킷 카운트용. 0으로 고정하면 CL에서 패킷 인식 안함
        internal byte Pkt219_Count { get; set; }
        //client unique id
        internal uint Uid { get; set; }
        //map이동 제한하기 위해 필요한 사용자 레벨, LockedMap.ini에서 지정
        internal byte Level { get; set; }
        //client가 어느 Zone에 있는지 확인, agent ID값
        internal byte ZoneStatus { get; set; }
        //client 접속 ip
        internal string IPadress { get; set; }
        //client 접속 account
        internal string Account { get; set; }
        //client의 현재 접속 캐릭터
        internal string Character { get; set; }
        //접속중인 캐릭터의 마을
        internal byte Town { get; set; }
        //클라이언트 마다의 고유 tick지정, 클라마다 다른 tick값 사용하기 위해
        internal int TickCltUnique { get; set; }
        //Tick Check
        internal uint TickCountPre { get; set; }
        internal uint TickCount { get; set; }
        internal uint TickSvr { get; set; }
        internal uint TickSvrPre { get; set; }
        internal uint TickCltPre { get; set; }
        internal int TickErrCount { get; set; }
        //로그 아웃 이유 체크
        internal byte Reason { get; set; }
        //클라 버전 체크
        internal ClientVer Ver { get; set; }
    }
}
