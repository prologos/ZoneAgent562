using System;
using System.Runtime.InteropServices;

namespace pConverter
{
    using DWORD = System.UInt32;
    using WORD = System.UInt16;
    using BYTE = System.Byte;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_1105_219 : Marshalling
    {
        public MSG_S2C_1105_219()
	    {
            MsgHeader = new MSG_S2C_HEADER();
            MsgHeader.dwSize = GetSize();//0x3B8;
		    MsgHeader.wProtocol = 0x1105;
            CharInfo = new CHARACTER_INFO219[5];
	    }
        public MSG_S2C_HEADER MsgHeader;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 5)]
	    public CHARACTER_INFO219[] CharInfo;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_1105_562 : Marshalling
    {
        public MSG_S2C_1105_562()
	    {
            MsgHeader = new MSG_S2C_HEADER();
            MsgHeader.dwSize = GetSize();//0x3B8;
		    MsgHeader.wProtocol = 0x1105;
            CharInfo = new CHARACTER_INFO562[5];
	    }
        public MSG_S2C_HEADER MsgHeader;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 5)]
	    public CHARACTER_INFO562[] CharInfo;
    }
}
