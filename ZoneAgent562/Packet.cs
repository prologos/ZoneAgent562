using pConverter;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ZoneAgent562
{
    /// <summary>
    /// LS에게 5초마다 보내는 현재 접속자수와 ZA상태
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZA2LS_REPORT : Marshalling
    {
        internal MSG_ZA2LS_REPORT()
	    {
            MsgHeader = new MSG_HEAD_NO_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x02;
            MsgHeader.byCmd = 0xE1;
        }
        internal MSG_HEAD_NO_PROTOCOL MsgHeader;
        internal uint dwPlayerCount;
        internal byte byZSCount1;
        internal byte byZSCount2;
    }
    /// <summary>
    /// LS와 연결 되었을때 처음 보내는 ZA의 정보
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZA2LS_CONNECT : Marshalling
    {
        internal MSG_ZA2LS_CONNECT()
	    {
            MsgHeader = new MSG_HEAD_NO_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x02;
            MsgHeader.byCmd = 0xE0;
        }
        internal MSG_HEAD_NO_PROTOCOL MsgHeader;
        internal byte byServerID;
        internal byte byAgentID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        internal string szZAIP;
        internal uint dwZAPort;
    }
    /// <summary>
    /// LS가 강제 종료 후 재 접속했을경우 LS에게 현재 접속자 정보를 알려주는 패킷
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZA2LS_LOGIN_USER_LIST : Marshalling
    {
        internal MSG_ZA2LS_LOGIN_USER_LIST()
        {
            MsgHeader = new MSG_HEAD_NO_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x02;
            MsgHeader.byCmd = 0xE4;
        }
        internal MSG_HEAD_NO_PROTOCOL MsgHeader;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        internal string szUserAccount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        internal string szUserIP;
        internal uint dwUnknown;
    }
    /// <summary>
    /// ZS들에게 처음 연결 되었을 경우 ZA(나)의 agent ID를 알려준다
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZA2ZS_CONNECT : Marshalling
    {
        internal MSG_ZA2ZS_CONNECT()
	    {
            MsgHeader = new MSG_HEAD_NO_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x01;
            MsgHeader.byCmd = 0xE0;
	    }
        internal MSG_HEAD_NO_PROTOCOL MsgHeader;
        internal byte byAgentID;
    }
    /// <summary>
    /// 유저가 로그아웃, 디스커넥트 되었을 경우 ZS에 이를 알려주는 패킷: AS포함 클라의 현재 ZoneStatus의 ZS에 보내면 된다.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZA2ZS_ACC_LOGOUT : Marshalling
    {
        internal MSG_ZA2ZS_ACC_LOGOUT()
	    {
		    MsgHeader = new MSG_HEAD_NO_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x01;
            MsgHeader.byCmd = 0xE2;
	    }
	    internal MSG_HEAD_NO_PROTOCOL MsgHeader;
	    internal byte byReason;
    }
    /// <summary>
    /// LS에게 사용자 로그아웃, 디스커넥트를 알린다
    /// Reason?: 0-로그아웃, 2-중복접속, 4-스핵
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZA2LS_ACC_LOGOUT : Marshalling
    {
        internal MSG_ZA2LS_ACC_LOGOUT()
	    {
		    MsgHeader = new MSG_HEAD_NO_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x02;
            MsgHeader.byCmd = 0xE2;
            szDate = DateTime.Now.ToString("yyyyMMdd");
            szTime = DateTime.Now.ToString("HHmmss");
	    }
	    internal MSG_HEAD_NO_PROTOCOL	MsgHeader;
	    internal byte	byReason;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
	    internal string szAccount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
	    private string szDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        private string szTime;
    }
    /// <summary>
    /// Prepare된 유저가 정상 로그인 했음을 LS에게 알려준다
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZA2LS_PREPARED_ACC_LOGIN : Marshalling
    {
        internal MSG_ZA2LS_PREPARED_ACC_LOGIN()
	    {
            MsgHeader = new MSG_HEAD_NO_PROTOCOL();
		    MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x02;
            MsgHeader.byCmd = 0xE3;
	    }
	    internal MSG_HEAD_NO_PROTOCOL MsgHeader;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
	    internal string szAccount;
    }
    /// <summary>
    /// AS에게 새 사용자를 알린다. 응답으로 캐릭터 리스트 패킷을 받는다
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZA2AS_NEW_CLIENT : Marshalling
    {
        internal MSG_ZA2AS_NEW_CLIENT()
	    {
		    MsgHeader = new MSG_HEAD_NO_PROTOCOL();
		    MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x01;
            MsgHeader.byCmd = 0xE1;
            szPassword = new char[21];
            szPassword[3] = Convert.ToChar(0x01);
	    }
	    internal MSG_HEAD_NO_PROTOCOL	MsgHeader;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
	    internal string szAccount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
	    internal char[] szPassword;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
	    internal string szClientIP;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 78)]
	    internal byte[]	byUnknow;
    }
    /// <summary>
    /// ZA - CL: TimeTick패킷, 스피드핵 탐지에 사용
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_ZACL_CHK_TIMETICK : Marshalling
    {
        internal MSG_ZACL_CHK_TIMETICK()
        {
            MsgHeader = new MSG_HEAD_NO_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x01;
            MsgHeader.byCmd = 0xF0;
        }
        internal MSG_HEAD_NO_PROTOCOL MsgHeader;
        internal uint dwTickCount;
        internal uint dwTickSvr;
        internal uint dwTickClt;
    }
    /// <summary>
    /// CL->ZA: 로그인 요청 패킷
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_CL2ZA_LOGIN : Marshalling
    {
        internal MSG_CL2ZA_LOGIN()
	    {
            MsgHeader = new MSG_HEAD_NO_PROTOCOL();
            MsgHeader.dwSize = GetSize();
		    MsgHeader.byCmd = 0xE2;
	    }

        internal MSG_HEAD_NO_PROTOCOL MsgHeader;
        internal uint dwPCID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        internal string szAccount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        internal string szPassword;
    }
    /// <summary>
    /// LS->ZA, 계정 로그인 성공한 Account정보를 알려준다(Prepare). 이후 사용자가 ZA에 로그인 시도한다
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_LS2ZA_ACC_LOGIN : Marshalling
    {
        internal MSG_LS2ZA_ACC_LOGIN()
	    {
		    MsgHeader = new MSG_HEAD_NO_PROTOCOL();
		    MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x01;
            MsgHeader.byCmd = 0xE1;
	    }
	    internal MSG_HEAD_NO_PROTOCOL	MsgHeader;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        internal string szAccount;
    }
    /// <summary>
    /// LS->ZA, 유저 디스커넥트 요청 패킷. Reason:2=중복 로그인
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal class MSG_LS2ZA_REQ_LOGOUT : Marshalling
    {
        internal MSG_LS2ZA_REQ_LOGOUT()
	    {
		    MsgHeader = new MSG_HEAD_NO_PROTOCOL();
		    MsgHeader.dwSize = GetSize();
            MsgHeader.byCtrl = 0x01;
            MsgHeader.byCmd = 0xE3;
            byReason = 0x02;
	    }
	    internal MSG_HEAD_NO_PROTOCOL	MsgHeader;
        internal byte byReason;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
	    internal string szAccount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        internal string szDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        internal string szTime;
    }

    internal static class Packet
    {
        internal static void SplitPackets(byte[] buffer, int length, out List<byte[]> packetList)
        {
            packetList = new List<byte[]>();
            byte[] packet;
            int offset = 0;
            int packetSize;

            while (length > offset)
            {
                packetSize = BitConverter.ToInt32(buffer, offset);
                if (packetSize <= 0 || packetSize > 2048 || offset + packetSize > length)
                    break;
                packet = new byte[packetSize];
                Array.Copy(buffer, offset, packet, 0, packetSize);
                packetList.Add(packet);
                offset += packetSize;
            }
        }

        public static byte[] SayMsg(uint uid, string msg, pSayType type)
        {
            MSG_S2C_HEADER header = new MSG_S2C_HEADER(uid, 0x1800);
            byte[] buffer = header.Serialize();
            buffer = ByteTools.CombineBytes(buffer, new byte[] { Convert.ToByte((byte)type) }, BitConverter.GetBytes(-1));
            if (type == pSayType.Notice)
                buffer = ByteTools.CombineBytes(buffer, ByteTools.String2Bytes("NOTICE", 0x15));
            else
                buffer = ByteTools.CombineBytes(buffer, ByteTools.String2Bytes("SYSTEM", 0x15));
            buffer = ByteTools.CombineBytes(buffer, ByteTools.String2Bytes(msg));
            BitConverter.GetBytes(buffer.Length).CopyTo(buffer, 0);
            return buffer;
        }
    }
}
