using System;

namespace pConverter
{
    public class Crypt
    {
        private int m_ConstKey1;
        private int m_ConstKey2;
        private int m_DynamicKey;
        private byte p_DynamicKey1;
        private byte p_DynamicKey2;
        private uint p_ConstKey_En;
        private uint p_ConstKey_De;

        public Crypt()
        {
            this.m_ConstKey1 = 0x241AE7;
            this.m_ConstKey2 = 0x15DCB2;
            this.m_DynamicKey = 0x4C478BD;
            this.p_DynamicKey1 = 0x02;
            this.p_DynamicKey2 = 0x01;
            this.p_ConstKey_En = 0xA7F0753B;
            this.p_ConstKey_De = 0xAAF29BF3;
        }

        public Crypt(int ConstKey1, int ConstKey2, int DynamicKey)
        {
            this.m_ConstKey1 = ConstKey1;
            this.m_ConstKey2 = ConstKey2;
            this.m_DynamicKey = DynamicKey;
            this.p_DynamicKey1 = 0x02;
            this.p_DynamicKey2 = 0x01;
            this.p_ConstKey_En = 0xA7F0753B;
            this.p_ConstKey_De = 0xAAF29BF3;
        }

        public Crypt(int ConstKey1, int ConstKey2, int DynamicKey, byte DynamicKey1, byte DynamicKey2, uint ConstKey_En, uint ConstKey_De)
        {
            this.m_ConstKey1 = ConstKey1;
            this.m_ConstKey2 = ConstKey2;
            this.m_DynamicKey = DynamicKey;
            this.p_DynamicKey1 = DynamicKey1;
            this.p_DynamicKey2 = DynamicKey2;
            this.p_ConstKey_En = ConstKey_En;
            this.p_ConstKey_De = ConstKey_De;
        }

        public void Decrypt(ref byte[] buffer, ClientVer cVer)
        {
            int sOffset = 0x0C;
            if (cVer == ClientVer.v578)
            {
                sOffset = 0x0E;
                uint uProtocol = (uint)((BitConverter.ToUInt32(buffer, 0x0A) * this.p_ConstKey_De) >> 0x10);
                Buffer.BlockCopy(BitConverter.GetBytes(uProtocol), 0, buffer, 10, 4);
            }
            for (int i = sOffset; i + 4 <= buffer.Length; i += 4)
            {
                int DynamicKey = this.m_DynamicKey;
                for (int j = i; j < i + 4; j++)
                {
                    byte pSrc = buffer[j];
                    buffer[j] = (byte)(buffer[j] ^ (DynamicKey >> 8));
                    DynamicKey = (pSrc + DynamicKey) * this.m_ConstKey1 + this.m_ConstKey2;
                }
            }
        }

        public void Encrypt(ref byte[] buffer, ClientVer cVer)
        {
            int sOffset = 0x0C;
            if (cVer == ClientVer.v578)
            {
                sOffset = 0x0E;
                uint uProtocol = (uint)(((BitConverter.ToUInt16(buffer, 0x0A) << 0x10) | (Environment.TickCount & 0xFFFF)) * this.p_ConstKey_En);
                Buffer.BlockCopy(BitConverter.GetBytes(uProtocol), 0, buffer, 10, 4);
            }
            for (int i = sOffset; i + 4 <= buffer.Length; i += 4)
            {
                int DynamicKey = this.m_DynamicKey;
                for (int j = i; j < i + 4; j++)
                {
                    buffer[j] = (byte)(buffer[j] ^ (DynamicKey >> 8));
                    DynamicKey = (buffer[j] + DynamicKey) * this.m_ConstKey1 + this.m_ConstKey2;
                }
            }
        }
    }
}
