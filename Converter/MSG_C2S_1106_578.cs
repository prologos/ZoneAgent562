using System;
using System.Runtime.InteropServices;

namespace pConverter
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_C2S_1106_578 : Marshalling
    {
        public MSG_C2S_1106_578()
        {
            MsgHeader = new MSG_HEAD_WITH_PROTOCOL578();
            MsgHeader.dwSize = GetSize();
            MsgHeader.dwProtocol = 0x1106;
        }

        public MSG_HEAD_WITH_PROTOCOL578 MsgHeader;
        public uint Unknown;
        public byte Unknown1;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public String szPCName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        public byte[] byUnknown;
    }
}
