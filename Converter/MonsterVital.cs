
namespace pConverter
{
    public class MonsterVital
    {
        private uint _MaxVital;
        private uint _CurrentVital;
        private byte _Percent;

        public MonsterVital(uint max, uint current)
        {
            this._MaxVital = max;
            this._CurrentVital = current;
            this._Percent = (byte)((float)current / max * 0xff);
        }

        public uint MaxVital
        {
            get { return this._MaxVital; }
        }
        public uint CurrentVital
        {
            get
            {
                return this._CurrentVital;
            }
            set
            {
                this._CurrentVital = value;
                this._Percent = (byte)((float)this._CurrentVital / this._MaxVital * 0xff);
            }
        }
        public byte Percent
        {
            get { return this._Percent; }
        }
    }
}
