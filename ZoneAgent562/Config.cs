using pConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ZoneAgent562
{
    internal class SvrInfo
    {
        internal byte sID { get; private set; }
        internal byte aID { get; private set; }
        internal IPAddress IP { get; private set; }
        internal int Port { get; private set; }
        internal string Status { get; set; }
        internal SvrInfo(byte sid, byte id, IPAddress ip, int port)
        {
            this.sID = sid;
            this.aID = id;
            this.IP = ip;
            this.Port = port;
            this.Status = "Disconnected";
        }
    }
    internal class MapInfo
    {
        internal int MapNum { get; private set; }
        internal byte MapLv { get; set; }
        internal MapInfo(int mNum, byte mLv)
        {
            this.MapNum = mNum;
            this.MapLv = mLv;
        }
    }
    
    internal static class Config
    {
        #region ini file access
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// read the key value in the ini file
        /// </summary>
        /// <param name="Section">Section String</param>
        /// <param name="Key">Key String</param>
        /// <param name="iniPath">ini File Path String</param>
        /// <param name="defaultValue">default Return Value String</param>
        private static string GetIniValue(string Section, string Key, string iniPath, string defaultValue = "")
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
            return (temp.ToString().Trim() != "") ? temp.ToString().Trim() : defaultValue;
        }
        #endregion

        internal static Msg_Convert mConvert;
        internal static byte AS_ID { get; private set; }
        internal static SvrInfo LS { get; private set; }
        internal static SvrInfo ZA { get; private set; }
        internal static Dictionary<byte, SvrInfo> ZSList { get; private set; }
        //텔레포트 제한
        internal static List<MapInfo> TeleportList { get; private set; }
        internal static Dictionary<string, byte> AccLevel { get; private set; }
        
        internal static bool Initialization(string Svrinfo, FrmMain frm)
        {
            try
            {
                //패킷 변환, encrypt, decrypt에 사용...
                mConvert = new Msg_Convert();

                //ZA
                byte sid = Convert.ToByte(GetIniValue("STARTUP", "SERVERID", Svrinfo, "0"));
                byte aid = Convert.ToByte(GetIniValue("STARTUP", "AGENTID", Svrinfo, "0"));
                IPAddress ip = IPAddress.Parse(GetIniValue("STARTUP", "IP", Svrinfo, "127.0.0.1"));
                int port = Convert.ToInt32(GetIniValue("STARTUP", "PORT", Svrinfo, "9981"));
                ZA = new SvrInfo(sid, aid, ip, port);

                ip = IPAddress.Parse(GetIniValue("LOGINSERVER", "IP", Svrinfo, "127.0.0.1"));
                port = Convert.ToInt32(GetIniValue("LOGINSERVER", "PORT", Svrinfo, "3200"));
                //LS는 ip, port만 사용하지만 Svr_Info 사용해서 생성
                LS = new SvrInfo(0, 0, ip, port);

                ZSList = new Dictionary<byte, SvrInfo>();
                //Add Account Server
                AS_ID = Convert.ToByte(GetIniValue("ACCOUNTSERVER", "ID", Svrinfo, "255"));
                ip = IPAddress.Parse(GetIniValue("ACCOUNTSERVER", "IP", Svrinfo, "127.0.0.1"));
                port = Convert.ToInt32(GetIniValue("ACCOUNTSERVER", "PORT", Svrinfo, "5589"));
                ZSList.Add(AS_ID, new SvrInfo(0, AS_ID, ip, port));
                //Add Battle Server
                aid = Convert.ToByte(GetIniValue("BATTLESERVER", "ID", Svrinfo, "3"));
                ip = IPAddress.Parse(GetIniValue("BATTLESERVER", "IP", Svrinfo, "127.0.0.1"));
                port = Convert.ToInt32(GetIniValue("BATTLESERVER", "PORT", Svrinfo, "6999"));
                ZSList.Add(aid, new SvrInfo(0, aid, ip, port));
                //Add Zone Server
                int zsCount = Convert.ToInt32(GetIniValue("ZONESERVER", "COUNT", Svrinfo, "1"));
                for (int i = 0; i < zsCount; i++)
                {
                    aid = Convert.ToByte(GetIniValue("ZONESERVER", "ID" + i, Svrinfo, i.ToString()));
                    ip = IPAddress.Parse(GetIniValue("ZONESERVER", "IP" + i, Svrinfo, "127.0.0.1"));
                    port = Convert.ToInt32(GetIniValue("ZONESERVER", "PORT" + i, Svrinfo, "6689"));
                    ZSList.Add(aid, new SvrInfo(0, aid, ip, port));
                }
                //Teleport file read
                AccLevel = new Dictionary<string, byte>();
                TeleportList = new List<MapInfo>();
                string TeleportFile = GetIniValue("TELEPORTFILE", "FULLPATH", Svrinfo);
                if (TeleportFile != "" && File.Exists(TeleportFile))
                {
                    using (var fs = new FileStream(TeleportFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
                    using (var sr = new StreamReader(fs, true))
                    {
                        string readLine;
                        while ((readLine = sr.ReadLine()) != null)
                        {
                            string[] temp = Regex.Split(readLine, @"\s+");
                            TeleportList.Add(new MapInfo(Convert.ToInt32(temp[0].Trim()), 0));
                        }
                    }
                }
                else
                {
                    frm.UpdateLogMsg("Load Teleport.txt:Error");
                    return false;
                }
                LoadLockedMap(frm);

                frm.UpdateZAinfo(ZA.sID, ZA.aID, ZA.Port);
            }
            catch
            {
                return false;
            }
            return true;
        }

        internal static bool LoadLockedMap(FrmMain frm)
        {
            string Svrinfo = @".\LockedMap.ini";
            if (File.Exists(Svrinfo))
            {
                try
                {
                    AccLevel.Clear();
                    //유저 레벨
                    int count = Convert.ToInt32(GetIniValue("USERLEVEL", "COUNT", Svrinfo, "0"));
                    for (int i = 0; i < count; i++)
                    {
                        string user = GetIniValue("USERLEVEL", "USER" + i, Svrinfo, ",");
                        string[] temp = user.Split(',');
                        AccLevel.Add(temp[0].Trim(), Convert.ToByte(temp[1].Trim()));
                    }
                    frm.UpdateLogMsg("Load UserLevel in LockedMap.ini:OK");
                    //유저 레벨 새로 읽었으니 로그인 유저들 레벨도 조정
                    if (ZoneAgent._Players != null && AccLevel.Count > 0)
                    {
                        lock (ZoneAgent._Players)
                        {
                            foreach (var user in ZoneAgent._Players)
                            {
                                if (AccLevel.ContainsKey(user.Value.Account))
                                    user.Value.Level = AccLevel[user.Value.Account];
                                else
                                    user.Value.Level = 0;
                            }
                        }
                        frm.UpdateLogMsg("=>Loggedin User Level readjust");
                    }

                    //맵 레벨, 텔레포트 리스트의 맵 레벨 재조정
                    foreach (var map in TeleportList)
                        map.MapLv = 0;
                    count = Convert.ToInt32(GetIniValue("MAPLEVEL", "COUNT", Svrinfo, "0"));
                    for (int i = 0; i < count; i++)
                    {
                        string user = GetIniValue("MAPLEVEL", "MAP" + i, Svrinfo, ",");
                        string[] temp = user.Split(',');
                        MapInfo map = TeleportList.Find(x => x.MapNum == Convert.ToInt32(temp[0].Trim()));
                        if (map != null)
                            map.MapLv = Convert.ToByte(temp[1].Trim());
                    }
                    frm.UpdateLogMsg("Load MapLevel in LockedMap.ini:OK");

                    return true;
                }
                catch
                {
                    frm.UpdateLogMsg("Load LockedMap.ini:Error");
                    return false;
                }
            }
            else
            {
                frm.UpdateLogMsg("LockedMap.ini not found");
                return false;
            }
        }
    }
}
