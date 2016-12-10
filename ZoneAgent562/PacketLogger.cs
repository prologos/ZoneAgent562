using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZoneAgent562
{
    static class PacketLogger
    {
        public static bool backupLogs = true;

        public static bool LogPacket(byte[] packet, string scenario, string character)
        {
            if (!Directory.Exists("PacketLogs"))
            {
                backupLogs = false;
                Directory.CreateDirectory("PacketLogs");
            }
            else
            {
                if (backupLogs)
                {
                    Directory.Move("PacketLogs", DateTime.Now.ToFileTime() + "-PacketLogsBackup");
                    backupLogs = false;
                }
                Directory.CreateDirectory("PacketLogs");
            }
            if (character == "")
                character = "misc";
            if (!Directory.Exists("PacketLogs/" + character))
                Directory.CreateDirectory("PacketLogs/" + character);
            BinaryWriter Writer = null;
            string Name = @"PacketLogs\" + character + "\\" + DateTime.Now.ToFileTime() + '_' + scenario + '_' + packet.Length + ".bin";
            try
            {
                Writer = new BinaryWriter(File.Open(Name, FileMode.Append));
                Writer.Write(packet);
                Writer.Flush();
                Writer.Close();
            }
            catch (Exception e)
            {
                StreamWriter sWriter = new StreamWriter(Directory.GetCurrentDirectory() + "/PacketLogginError.txt", true);
                sWriter.WriteLine(e.Message);
                sWriter.Close();
                return false;
            }
            return true;
        }
    }
}
