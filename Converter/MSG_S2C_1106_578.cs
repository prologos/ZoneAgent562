using System;
using System.Runtime.InteropServices;

namespace pConverter
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_1106_578 : Marshalling
    {
        public MSG_S2C_1106_578()
        {
            MsgHeader = new MSG_S2C_HEADER_578();
            MsgHeader.dwSize = GetSize();
            MsgHeader.dwProtocol = 0x1106;
        }

        public MSG_S2C_HEADER_578 MsgHeader;
        public byte Unknown;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public string szPCName;

        public uint RandomNumer;
        public ushort Unknown1;
        public ushort Map;
    }
}
