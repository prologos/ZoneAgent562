using System.Runtime.InteropServices;

namespace pConverter
{
    using DWORD = System.UInt32;
    using WORD = System.UInt16;
    using BYTE = System.Byte;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_1107_578 : Marshalling
    {
        public MSG_S2C_1107_578()
        {
            MsgHeader = new MSG_S2C_HEADER_578();
            MsgHeader.dwSize = GetSize();//0x42C;
            MsgHeader.dwProtocol = 0x1107;
        }

        public MSG_S2C_HEADER_578 MsgHeader;
        public WORD aUnknow2;//2
        public WORD wMP;//4
        public WORD wStr;//5 - 6
        public WORD wMagic;//7 - 8
        public WORD wDex;     //9 - 10
        public WORD wVit;//11 - 12
        public WORD wMana;//13- 14
        public DWORD dwMaxStoreHP;
        public DWORD dwMaxStoreMP;
        public WORD wHP;//23 - 24
        public WORD wPoint;//26
        public WORD wMinATK;
        public WORD wMinMATK;
        public WORD wDef;
        public WORD wFireAtk;
        public WORD wFireDef;
        public WORD wIceAtk; //116
        public WORD wIceDef; //118
        public WORD wRightAtk; //120
        public WORD wRightDef; //124
        public WORD wMaxHp;//45 - 46
        public WORD wMaxMp;//47 - 48
        public WORD wMaxATK;
        public WORD wMaxMATK;
        public WORD wUnknowStat; //54

        //PET Act start
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public BYTE[] byPetAct;//74

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]//75 - 81
        public string szPCName;//81

        public BYTE byClass;//82
        public WORD wLevel;//84
        public DWORD dwExp;//88
        public DWORD dwMapIndex;//82
        public DWORD dwMapcell;//86

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        public BYTE[] bySkill;//110

        public BYTE byTown;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public BYTE[] SInfo;

        public DWORD Woonz;//122
        public DWORD dwStoreHp;//126
        public DWORD dwStoreMP;//132
        public DWORD dwLorePoint;//136
        public WORD wUnk;
        public BYTE byUnx;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public BYTE[] byPetInven;

        public BYTE byUnKnowx;

        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x3C)]
        //public ITEMINFO[] mInven;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 600)]//263 - 1462
        public BYTE[] Inven;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 600)]//263 - 1462
        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x1E)]
        //public ITEMINFO[] CharInven1;
        public BYTE unKnow2;//1661 - 1662

        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x0A)]
        //public ITEMWITHOPTION[] WearList;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]//1463 - 1622
        public BYTE[] WearList;
    }
}
