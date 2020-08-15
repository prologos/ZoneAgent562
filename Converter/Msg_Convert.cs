using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace pConverter
{
    public class Msg_Convert
    {
        public Crypt Crypter;

        public Msg_Convert()
        {
            Crypter = new Crypt();
        }

        public Msg_Convert(int ConstKey1, int ConstKey2, int DynamicKey)
        {
            Crypter = new Crypt(ConstKey1, ConstKey2, DynamicKey);
        }

        /// <summary>
        /// CL->ZS: 패킷 Decrypt후 필요한 경우 578버전으로 형 변환 한다
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="Uid"></param>
        /// <param name="cVer"></param>
        /// <param name="cltIP"></param>
        public void Convert_C2S(ref byte[] buffer, uint Uid, ClientVer cVer, string cltIP)
        {
            try
            {
                //클라에서 오는 패킷은 uid가 비어있으므로 uid추가 해준 후 decrypt
                ByteTools.SetPCID(ref buffer, Uid);
                Crypter.Decrypt(ref buffer, cVer);
                MSG_HEAD_WITH_PROTOCOL pHeader = new MSG_HEAD_WITH_PROTOCOL();
                pHeader.Deserialize(ref buffer);

                if (cVer == ClientVer.v578)
                {
                    Type type = Type.GetType(string.Format("pConverter.Ox{0}", pHeader.wProtocol.ToString("X4")));
                    if (type != null)
                    {
                        object dClass = Activator.CreateInstance(type);
                        MethodInfo C2S_TO219 = type.GetMethod("C2S_TO219");
                        object[] args = new object[] { buffer };
                        C2S_TO219.Invoke(dClass, args);
                        buffer = (byte[])args[0];
                    }
                    else
                    {
                        Logger.NewPacket("C2S", buffer, cltIP, cVer);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("Convert_C2S: {0}", ex));
            }
        }

        /// <summary>
        /// ZS->CL: 형 변환이 필요한경우 578버전으로 바꾼 후 패킷을 Encrypt한다
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="cVer"></param>
        public void Convert_S2C(ref byte[] buffer, ClientVer cVer)
        {
            try
            {
                MSG_HEAD_WITH_PROTOCOL pHeader = new MSG_HEAD_WITH_PROTOCOL();
                pHeader.Deserialize(ref buffer);

                if (pHeader.byCtrl != 0x03)
                    return;
                if (cVer == ClientVer.v578)
                {
                    Type type = Type.GetType(string.Format("pConverter.Ox{0}", pHeader.wProtocol.ToString("X4")));
                    if (type != null)
                    {
                        object dClass = Activator.CreateInstance(type);
                        MethodInfo S2C_TO578 = type.GetMethod("S2C_TO578");
                        object[] args = new object[] { buffer };
                        S2C_TO578.Invoke(dClass, args);
                        buffer = (byte[])args[0];
                    }
                    else
                    {
                        Logger.NewPacket("S2C", buffer, "localhost", cVer);
                    }
                }
                else if (pHeader.wProtocol == 0x1105 && cVer == ClientVer.v562)
                {
                    new Ox1105().S2C_TO562(ref buffer);
                }
                Crypter.Encrypt(ref buffer, cVer);
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("Convert_S2C: {0}", ex));
            }
        }
    }

    public class Ox1105
    {
        public void S2C_TO578(ref byte[] buffer)
        {
            MSG_S2C_1105_219 pMsg219 = new MSG_S2C_1105_219();
            MSG_S2C_1105_578 pMsg578 = new MSG_S2C_1105_578();
            pMsg219.Deserialize(ref buffer);
            pMsg578.MsgHeader.dwPCID = pMsg219.MsgHeader.dwPCID;
            for (int i = 0; i < 5; i++)
            {
                pMsg578.CharInfo[i].szPCName = pMsg219.CharInfo[i].szPCName;
                pMsg578.CharInfo[i].byClass = pMsg219.CharInfo[i].byClass;
                pMsg578.CharInfo[i].byTown = pMsg219.CharInfo[i].byTown;
                pMsg578.CharInfo[i].byUsed = 1;
                pMsg578.CharInfo[i].dwLevel = pMsg219.CharInfo[i].dwLevel;
                pMsg578.CharInfo[i].WearList = pMsg219.CharInfo[i].WearList;
            }
            Array.Resize(ref buffer, (int)pMsg578.GetSize());
            buffer = pMsg578.Serialize();
        }

        public void S2C_TO562(ref byte[] buffer)
        {
            MSG_S2C_1105_219 pMsg219 = new MSG_S2C_1105_219();
            MSG_S2C_1105_562 pMsg562 = new MSG_S2C_1105_562();
            pMsg219.Deserialize(ref buffer);
            pMsg562.MsgHeader.dwPCID = pMsg219.MsgHeader.dwPCID;
            for (int i = 0; i < 5; i++)
            {
                pMsg562.CharInfo[i].szPCName = pMsg219.CharInfo[i].szPCName;
                pMsg562.CharInfo[i].byClass = pMsg219.CharInfo[i].byClass;
                pMsg562.CharInfo[i].byTown = pMsg219.CharInfo[i].byTown;
                pMsg562.CharInfo[i].byUsed = 1;
                pMsg562.CharInfo[i].dwLevel = pMsg219.CharInfo[i].dwLevel;
                pMsg562.CharInfo[i].WearList = pMsg219.CharInfo[i].WearList;
            }
            Array.Resize(ref buffer, (int)pMsg562.GetSize());
            buffer = pMsg562.Serialize();
        }
    }

    public class Ox1106
    {
        public void C2S_TO219(ref byte[] buffer)
        {
            MSG_C2S_1106_578 pMsg578 = new MSG_C2S_1106_578();
            MSG_C2S_1106_219 pMsg219 = new MSG_C2S_1106_219();
            pMsg578.Deserialize(ref buffer);
            pMsg219.szPCName = pMsg578.szPCName;
            pMsg219.MsgHeader.dwPCID = pMsg578.MsgHeader.dwPCID;
            buffer = pMsg219.Serialize();
        }

        public void S2C_TO578(ref byte[] buffer)
        {
            MSG_S2C_1106_219 pMsg219 = new MSG_S2C_1106_219();
            MSG_S2C_1106_578 pMsg578 = new MSG_S2C_1106_578();
            pMsg219.Deserialize(ref buffer);
            pMsg578.MsgHeader.dwPCID = pMsg219.MsgHeader.dwPCID;
            pMsg578.szPCName = pMsg219.szPCName;
            pMsg578.RandomNumer = pMsg219.RandomNumer;
            pMsg578.Map = pMsg219.Map;
            buffer = pMsg578.Serialize();
        }
    }

    public class Ox1107
    {
        public void C2S_TO219(ref byte[] buffer)
        {
            MSG_C2S_1107_578 pMsg578 = new MSG_C2S_1107_578();
            MSG_C2S_1107_219 pMsg219 = new MSG_C2S_1107_219();
            pMsg578.Deserialize(ref buffer);
            pMsg219.szPCName = pMsg578.szPCName;
            pMsg219.MsgHeader.dwPCID = pMsg578.MsgHeader.dwPCID;
            buffer = pMsg219.Serialize();
        }

        public void S2C_TO578(ref byte[] buffer)
        {
            MSG_S2C_1107_219 pMsg219 = new MSG_S2C_1107_219();
            MSG_S2C_1107_578 pMsg578 = new MSG_S2C_1107_578();
            pMsg219.Deserialize(ref buffer);
            pMsg578.MsgHeader.dwPCID = pMsg219.MsgHeader.dwPCID;
            pMsg578.szPCName = pMsg219.Name;
            pMsg578.wMP = pMsg219.MP;
            pMsg578.wStr = pMsg219.Str;
            pMsg578.wMagic = pMsg219.Magic;
            pMsg578.wDex = pMsg219.Dex;
            pMsg578.wVit = pMsg219.Vit;
            pMsg578.wMana = pMsg219.Mana;
            pMsg578.dwMaxStoreHP = pMsg219.StoredHp;
            pMsg578.dwMaxStoreMP = pMsg219.StoredMp;
            pMsg578.wHP = pMsg219.HP;
            pMsg578.wPoint = pMsg219.Point;
            pMsg578.wMinATK = pMsg219.HitAttack;
            pMsg578.wMinMATK = pMsg219.MagicAttack;
            pMsg578.wDef = pMsg219.Defense;
            pMsg578.wFireAtk = pMsg219.FireAttack;
            pMsg578.wFireDef = pMsg219.FireDefence;
            pMsg578.wIceAtk = pMsg219.IceAttack;
            pMsg578.wIceDef = pMsg219.IceDefense;
            pMsg578.wRightAtk = pMsg219.LightAttack;
            pMsg578.wRightDef = pMsg219.LightDefense;
            pMsg578.wMaxHp = pMsg219.MaxHp;
            pMsg578.wMaxMp = pMsg219.MaxMp;
            pMsg578.wMaxATK = pMsg219.HitAddition;
            pMsg578.wMaxMATK = pMsg219.MagAddition;
            //pMsg578.Inven = pMsg219.Inventory;
            pMsg578.byTown = (byte)pMsg219.Nation;
            pMsg578.byPetAct = pMsg219.PetActive;
            pMsg578.byPetInven = pMsg219.PetInventory;
            //pMsg578.WearList = pMsg219.WearList;
            pMsg578.byClass = pMsg219.Type;
            pMsg578.wLevel = pMsg219.Level;
            pMsg578.dwExp = pMsg219.Exp;
            pMsg578.dwMapIndex = pMsg219.MapIndex;
            pMsg578.dwMapcell = pMsg219.CellIndex;
            pMsg578.Woonz = pMsg219.Money;
            pMsg578.dwStoreHp = pMsg219.StoredHp;
            pMsg578.dwStoreMP = pMsg219.StoredMp;
            pMsg578.dwLorePoint = pMsg219.Lore;
            buffer = pMsg578.Serialize();
        }
    }
}
