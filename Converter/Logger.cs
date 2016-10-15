using System;
using System.IO;

namespace pConverter
{
    public class Logger
    {
        public static void Write(string log)
        {
            try
            {
                if (!Directory.Exists("./SystemLog"))
                    Directory.CreateDirectory("./SystemLog");
                using (StreamWriter sw = new StreamWriter(string.Format("./SystemLog/ZoneAgent{0}.log", DateTime.Now.ToString("yyyy-MM-dd")), true))
                {
                    sw.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), log));
                }
            }
            catch { }
        }

        public static void WriteSDB(string log)
        {
            try
            {
                if (!Directory.Exists("./Log"))
                    Directory.CreateDirectory("./Log");
                string logFile = string.Format("./Log/{0}.sdb", DateTime.Now.ToString("yyyy-MM-dd"));
                using (StreamWriter sw = new StreamWriter(logFile, true))
                {
                    FileInfo fileInfo = new FileInfo(logFile);
                    if (fileInfo.Length == 0)
                        sw.WriteLine("Time,  Type,  ID,  CharName,  IP,  SvrTick,  CltTick,  Ping");
                    sw.WriteLine(string.Format("[{0}]  {1}", DateTime.Now.ToString("HH:mm:ss"), log));
                }
            }
            catch { }
        }

        public static void NewPacket(string act, byte[] packet, string ip, ClientVer cVer)
        {
            try
            {
                if (!Directory.Exists("./NewPacket"))
                    Directory.CreateDirectory("./NewPacket");

                string sPacket = BitConverter.ToString(packet).Replace("-", " ");
                using (StreamWriter sw = new StreamWriter(string.Format("./NewPacket/uPacket_{0}.log", DateTime.Now.ToString("yyyy-MM-dd")), true))
                {
                    sw.WriteLine(string.Format("Action: {0}", act));
                    sw.WriteLine(string.Format("ClientVer: {0}", cVer.ToString()));
                    sw.WriteLine(string.Format("ClientIP: {0}", ip));
                    if (sPacket.Length >= 35)
                        sw.WriteLine(string.Format("Protocol: 0x{0}{1}", sPacket.Substring(33, 2), sPacket.Substring(30, 2)));
                    else
                        sw.WriteLine("Protocol: 0x----");
                    sw.WriteLine("00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F");
                    sw.WriteLine("-----------------------------------------------");
                    for (int i = 0; i < sPacket.Length; i++)
                    {
                        if (i > 0 && i % 48 == 0)
                            sw.Write("\r\n");
                        sw.Write(sPacket[i]);
                    }
                    sw.Write("\r\n");
                    sw.Write("\r\n");
                }
            }
            catch { }
        }
    }
}
