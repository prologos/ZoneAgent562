using System.Runtime.InteropServices;

namespace pConverter
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_C2S_1107_578 : Marshalling
    {
        public MSG_C2S_1107_578()
        {
            MsgHeader = new MSG_HEAD_WITH_PROTOCOL578();
            MsgHeader.dwSize = GetSize();
            MsgHeader.dwProtocol = 0x1107;
        }

        public MSG_HEAD_WITH_PROTOCOL578 MsgHeader;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x15)]
        public string szPCName;

        public uint dwMapIndex;
    }
}
