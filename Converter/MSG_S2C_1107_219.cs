using System.Runtime.InteropServices;

namespace pConverter
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_S2C_1107_219 : Marshalling
    {
        public MSG_S2C_1107_219()
        {
            MsgHeader = new MSG_S2C_HEADER();
            MsgHeader.dwSize = GetSize();
            MsgHeader.wProtocol = 0x1107;
        }

        public MSG_S2C_HEADER MsgHeader;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string Name;

        public byte Type;
        public ushort Level;
        public uint Exp;
        public uint MapIndex;
        public uint CellIndex;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] Skill;

        public uint PKCount;
        public uint RTime;
        public uint Rank;
        public uint KnightIndex;
        public uint Nation;
        public uint Money;
        public uint StoredHp;
        public uint StoredMp;
        public uint Lore;
        public ushort Point;
        public ushort Str;
        public ushort Magic;
        public ushort Dex;
        public ushort Vit;
        public ushort Mana;
        public uint HPCapacity;
        public uint MPCapacity;
        public ushort HP;
        public ushort MP;
        public ushort HitAttack;
        public ushort MagicAttack;
        public ushort Defense;
        public ushort FireAttack;
        public ushort FireDefence;
        public ushort IceAttack;
        public ushort IceDefense;
        public ushort LightAttack;
        public ushort LightDefense;
        public ushort MaxHp;
        public ushort MaxMp;
        public ushort HitAddition;
        public ushort MagAddition;
        public ushort unknown;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
        public byte[] WearList;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 600)]
        public byte[] Inventory;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] PetActive;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public byte[] PetInventory;
    }
}
