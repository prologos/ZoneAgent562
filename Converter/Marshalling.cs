using System;
using System.Runtime.InteropServices;

namespace pConverter
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Marshalling
    {
        /// <summary>
        /// 구조체 -> 바이트 배열
        /// </summary>
        /// <returns></returns>
        public byte[] Serialize()
        {
            // allocate a byte array for the struct data
            byte[] buffer = new byte[Marshal.SizeOf(this)];

            // Allocate a GCHandle and get the array pointer
            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr pBuffer = gch.AddrOfPinnedObject();

            // copy data from struct to array and unpin the gc pointer
            Marshal.StructureToPtr(this, pBuffer, false);
            gch.Free();

            return buffer;
        }
        /// <summary>
        /// 바이트 배열 -> 구조체
        /// </summary>
        /// <param name="buffer"></param>
        public void Deserialize(ref byte[] buffer)
        {
            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.PtrToStructure(gch.AddrOfPinnedObject(), this);
            gch.Free();
        }

        public uint GetSize()
        {
            return (uint)Marshal.SizeOf(this);
        }
    }
}
