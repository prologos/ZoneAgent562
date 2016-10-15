using System;

namespace pConverter
{
    public enum ClientVer
    {
        undefined = 0x00,
        v219 = 0x42,
        v562 = 0x38,
        v578 = 0x44
    }
    public enum pSayType : byte
    {
        Notice = 0x0C,
        System = 0x00
    }
}
