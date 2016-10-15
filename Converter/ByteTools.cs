using System;
using System.Text;

namespace pConverter
{
    public class ByteTools
    {
        //Add Uid
        public static void SetPCID(ref byte[] buffer, uint uid)
        {
            Array.Copy(BitConverter.GetBytes(uid), 0, buffer, 4, 4);
        }

        //Combine ByteArray
        public static byte[] CombineBytes(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
        public static byte[] CombineBytes(byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length, third.Length);
            return ret;
        }

        //String to ByteArray
        public static byte[] String2Bytes(string str)
        {
            str += '\0';
            byte[] bytearray = System.Text.Encoding.Default.GetBytes(str);
            return bytearray;
        }
        public static byte[] String2Bytes(string str, int length)
        {
            byte[] bytearray = new byte[length];
            byte[] stringarray = System.Text.Encoding.Default.GetBytes(str);
            if (stringarray.Length < length)
            {
                stringarray.CopyTo(bytearray, 0);
            }
            else
            {
                Array.Copy(stringarray, bytearray, length - 1);
            }
            return bytearray;
        }

        public static string Bytes2String(byte[] packet, int offset, int count = 21)
        {
            string str = Encoding.Default.GetString(packet, offset, count).TrimEnd('\0');
            return getTrimmedString(str);
        }

        public static string getTrimmedString(string str)
        {
            string[] strArray = str.Split('\0');
            return strArray[0];
        }

        public static string bSubString(string str, int bytes)
        {
            byte[] bytearray = new byte[bytes];
            byte[] stringarray = System.Text.Encoding.Default.GetBytes(str);
            if (stringarray.Length < bytes)
                stringarray.CopyTo(bytearray, 0);
            else
                Array.Copy(stringarray, bytearray, bytes - 1);
            return Encoding.Default.GetString(bytearray).TrimEnd('\0');
        }
    }
}
