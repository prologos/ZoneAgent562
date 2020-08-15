using System.Runtime.InteropServices;

namespace pConverter
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_C2S_1106_219 : Marshalling
    {
        public MSG_C2S_1106_219()
        {
            MsgHeader = new MSG_HEAD_WITH_PROTOCOL();
            MsgHeader.dwSize = GetSize();
            MsgHeader.wProtocol = 0x1106;
            Unknown = 0x0243;
        }

        public MSG_HEAD_WITH_PROTOCOL MsgHeader;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public string szPCName;

        public uint Unknown;
    }
}
