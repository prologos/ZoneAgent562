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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FRIEND_GROUP_LIST
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szGroupName;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FRIEND_LIST
    {
        public BYTE byIdx;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szGroupName;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TARGET_LIST
    {
        public DWORD byIdx;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x06)]
        public BYTE[] byTarget;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MERCENARYINFO
    {
        public DWORD byIdx;
        public BYTE Class;
        public BYTE status;
        public WORD Level;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String MercName;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ITEM
    {
        public DWORD ItemId;
        public DWORD itemCode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ITEMINWEAR
    {
        public DWORD itemOpt1;
        public DWORD itemOpt2;
        public DWORD ItemId;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ITEMWITHOPTION
    {
        public DWORD ItemId;
        public DWORD itemCode;
        public DWORD itemOption;
        public DWORD itemOptionEx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ITEMINFO
    {

        public DWORD ItemId;
        public DWORD itemCode;
        public DWORD itemOption;
        public DWORD itemOptionEx;
        public DWORD storageIdx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PETINFO
    {
        public DWORD PetId;
        public DWORD itemCode;
        public DWORD itemOption;
        public DWORD itemOptionEx;
        public DWORD storageIdx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_CHAR_LIST_578 : Marshalling
    {
        public MSG_S2C_CHAR_LIST_578()
        {
            MsgHeader = new MSG_S2C_HEADER_578();
            MsgHeader.dwSize = GetSize();//0x3BF;
            MsgHeader.dwProtocol = 0x1105;
            byUnknow = new BYTE[0x05];
            CharInfo = new CHARACTER_INFO562[5];
        }
        public MSG_S2C_HEADER_578 MsgHeader;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public BYTE[] byUnknow;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 5)]
        public CHARACTER_INFO562[] CharInfo;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_CHAR_LIST : Marshalling
    {
        public MSG_S2C_CHAR_LIST()
        {
            MsgHeader = new MSG_S2C_HEADER_578();
            MsgHeader.dwSize = GetSize();//0x3BF;
            MsgHeader.dwProtocol = 0x1105;
            byUnknow = new BYTE[0x05];
        }
        public MSG_S2C_HEADER_578 MsgHeader;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public BYTE[] byUnknow;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 940)]
        public BYTE[] CharInfo;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_C2S_CHAR_LOGIN : Marshalling
    {
        public MSG_C2S_CHAR_LOGIN()
        {
            MsgHeader = new MSG_S2C_HEADER_578();
            MsgHeader.dwSize = GetSize();//0x42C;
            MsgHeader.dwProtocol = 0x1106;
        }
        public MSG_S2C_HEADER_578 MsgHeader;
        public DWORD CurVersion;
        public BYTE byUnknow;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szPCName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x18)]
        public BYTE[] WearList;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_CHAR_LOGIN_OK : Marshalling
    {
        public MSG_S2C_CHAR_LOGIN_OK()
        {
            MsgHeader = new MSG_S2C_HEADER_578();
            MsgHeader.dwSize = GetSize();//0x42C;
            MsgHeader.dwProtocol = 0x1106;
        }
        public MSG_S2C_HEADER_578 MsgHeader;
        public BYTE byCtrl;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szPCName;
        public DWORD CurVersion;
        public WORD byNation;
        public WORD byTown;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_C2S_WORLD_LOGIN : Marshalling
    {
        public MSG_C2S_WORLD_LOGIN()
        {
            MsgHeader = new MSG_S2C_HEADER_578();
            MsgHeader.dwSize = GetSize();//0x42C;
            MsgHeader.dwProtocol = 0x1107;
        }
        public MSG_S2C_HEADER_578 MsgHeader;
        public BYTE byClass;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szPCName;
        public DWORD dwMapIndex;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_WORLD_LOGIN_578 : Marshalling
    {
        public MSG_S2C_WORLD_LOGIN_578()
        {
            MsgHeader = new MSG_S2C_HEADER_578();
            MsgHeader.dwSize = GetSize();//0x42C;
            MsgHeader.dwProtocol = 0x1107;
        }
        public MSG_S2C_HEADER_578 MsgHeader;
        //You can make this from database select. I only put the packet for test.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1663)]
        public BYTE[] TestByte;
        //-----------------------------------------------------------------------

        //public WORD aUnknow2;//2
        //public WORD wMP;//4      
        //public WORD wStr;//5 - 6
        //public WORD wMagic;//7 - 8
        //public WORD wDex;     //9 - 10 
        //public WORD wVit;//11 - 12
        //public WORD wMana;//13- 14
        //public DWORD dwMaxStoreHP;
        //public DWORD dwMaxStoreMP;
        //public WORD wHP;//23 - 24
        //public WORD wPoint;//26   
        //public WORD wMinATK;
        //public WORD wMinMATK;
        //public WORD wDef;
        //public WORD wFireAtk;
        //public WORD wFireDef;
        //public WORD wIceAtk; //116
        //public WORD wIceDef; //118
        //public WORD wRightAtk; //120
        //public WORD wRightDef; //124
        //public WORD wMaxHp;//45 - 46
        //public WORD wMaxMp;//47 - 48
        //public WORD wMaxATK;
        //public WORD wMaxMATK;
        //public WORD wUnknowStat; //54
        ////PET Act start   
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        //public BYTE[] byPetAct;//74
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]//75 - 81
        //public String szPCName;//81
        //public BYTE byClass;//82
        //public WORD wLevel;//84     
        //public DWORD dwExp;//88
        //public DWORD dwMapIndex;//82
        //public DWORD dwMapcell;//86
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        //public BYTE[] bySkill;//110
        //public BYTE byTown;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        //public BYTE[] SInfo;
        //public DWORD Woonz;//122
        //public DWORD dwStoreHp;//126       
        //public DWORD dwStoreMP;//132       
        //public DWORD dwLorePoint;//136
        //public WORD wUnk;
        //public BYTE byUnx;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        //public BYTE[] byPetInven;
        //public BYTE byUnKnowx;
        ////[MarshalAs(UnmanagedType.ByValArray, SizeConst = 600)]//263 - 1462
        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x3C)]
        //public ITEMINFO[] mInven;
        ////[MarshalAs(UnmanagedType.ByValArray, SizeConst = 600)]//263 - 1462
        ////[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x1E)]
        ////public ITEMINFO[] CharInven1;
        //public BYTE unKnow2;//1661 - 1662
        ////[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]//1463 - 1622
        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x0A)]
        //public ITEMWITHOPTION[] WearList;
    }
}
