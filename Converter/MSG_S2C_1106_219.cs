using System.Runtime.InteropServices;

namespace pConverter
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_1106_219 : Marshalling
    {
        public MSG_S2C_1106_219()
        {
            MsgHeader = new MSG_S2C_HEADER();
            MsgHeader.dwSize = GetSize();
            MsgHeader.wProtocol = 0x1106;
        }

        public MSG_S2C_HEADER MsgHeader;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public string szPCName;

        public uint RandomNumer;
        public ushort Map;
    }
}
