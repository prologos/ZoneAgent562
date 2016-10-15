using System;
using System.Runtime.InteropServices;

namespace pConverter
{
    using DWORD = System.UInt32;
    using WORD = System.UInt16;
    using BYTE = System.Byte;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_HEAD_WITH_PROTOCOL : Marshalling
    {
        public DWORD dwSize;
        public DWORD dwPCID;
        public BYTE byCtrl;
        public BYTE byCmd;
        public WORD wProtocol;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_HEAD_WITH_PROTOCOL578 : Marshalling
    {
        public DWORD dwSize;
        public DWORD dwPCID;
        public BYTE byCtrl;
        public BYTE byCmd;
        public DWORD dwProtocol;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_HEAD_NO_PROTOCOL : Marshalling
    {
        public DWORD dwSize;
        public DWORD dwPCID;
        public BYTE byCtrl;
        public BYTE byCmd;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_HEADER : Marshalling
    {
        public MSG_S2C_HEADER()
        {
            byCtrl = 0x03;
            byCmd = 0xFF;
        }
        public MSG_S2C_HEADER(DWORD uid, WORD protocol)
        {
            dwPCID = uid;
            byCtrl = 0x03;
            byCmd = 0xFF;
            wProtocol = protocol;
        }
        public DWORD dwSize;
        public DWORD dwPCID;
        public BYTE byCtrl;
        public BYTE byCmd;
        public WORD wProtocol;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_HEADER_578 : Marshalling
    {
        public MSG_S2C_HEADER_578()
        {
            byCtrl = 0x03;
            byCmd = 0xFF;
        }
        public DWORD dwSize;
        public DWORD dwPCID;
        public BYTE byCtrl;
        public BYTE byCmd;
        public DWORD dwProtocol;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_SAY : Marshalling
    {
        public MSG_S2C_SAY()
        {
            MsgHeader = new MSG_S2C_HEADER();
            MsgHeader.dwSize = GetSize();
            MsgHeader.wProtocol = 0x1800;
            bySayType = (byte)pSayType.System;
            dwSayPCID = uint.MaxValue;
            szSayPC = "SYSTEM";
            szWords = string.Empty;
        }

        public MSG_S2C_HEADER MsgHeader;
        public BYTE bySayType;
        public DWORD dwSayPCID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szSayPC;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)]
        public String szWords;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_C2S_REQ_TELEPORT : Marshalling
    {
        public MSG_C2S_REQ_TELEPORT()
        {
            MsgHeader = new MSG_HEAD_WITH_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.wProtocol = 0x1112;
        }
        public MSG_HEAD_WITH_PROTOCOL MsgHeader;
        public DWORD dwNPCID;
        public WORD wTargetLine;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_WORLD_LOGIN : Marshalling
    {
        public MSG_S2C_WORLD_LOGIN()
        {
            MsgHeader = new MSG_S2C_HEADER();
            MsgHeader.dwSize = GetSize();//0x42C;
            MsgHeader.wProtocol = 0x1107;
        }
        public MSG_S2C_HEADER MsgHeader;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szPCName;
        public BYTE byClass;
        public WORD wLevel;
        public DWORD dwExp;
        public DWORD dwMapNum;
        public DWORD dwXY;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1C)]
        public BYTE[] m30_by;
        public BYTE byTown;
    }

    /// <summary>
    /// ACL패킷중 각 슬롯의 자료형, 총 5개의 슬롯이 사용된다.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CHARACTER_INFO219
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szPCName;
        public BYTE byClass;
        public BYTE byTown;
        public BYTE byRev2;
        public DWORD dwLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xA0)]
        public BYTE[] WearList;
    }
    /// <summary>
    /// ACL패킷중 각 슬롯의 자료형, 총 5개의 슬롯이 사용된다.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CHARACTER_INFO562
    {
	    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szPCName;
        public BYTE byUsed;
        public BYTE byClass;
        public BYTE byTown;
        public DWORD dwLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xA0)]
        public BYTE[] WearList;
    }
}
