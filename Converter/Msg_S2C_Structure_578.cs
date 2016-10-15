using System;
using System.Runtime.InteropServices;

namespace pConverter
{
    using DWORD = System.UInt32;
    using WORD = System.UInt16;
    using BYTE = System.Byte;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_1105_578 : Marshalling
    {
        public MSG_S2C_1105_578()
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
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.Struct, SizeConst = 5)]
        public CHARACTER_INFO562[] CharInfo;
    }
}
