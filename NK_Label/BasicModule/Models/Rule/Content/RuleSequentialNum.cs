using BasicModule.Common;

namespace BasicModule.Models.Rule.Content
{
    public class RuleSequentialNum : NotifyPropertyChanged, IRuleObject
    {
        #region Properties

        private byte _numLength = 2;
        public byte NumLength
        {
            get { return _numLength; }
            set
            {
                if (value >= ulong.MinValue.ToString("D").Length && value <= ulong.MaxValue.ToString("D").Length)
                {
                    _numLength = value;
                    OnPropertyChanged();
                }
            }
        }

        private ulong _minNum;
        public ulong MinNum
        {
            get { return _minNum; }
            set
            {
                if (value <= MaxNum && value.ToString("D").Length <= NumLength)
                {
                    _minNum = value;
                    OnPropertyChanged();

                    if (CurrNum < _minNum)
                        CurrNum = _minNum;
                }
            }
        }

        private ulong _maxNum;
        public ulong MaxNum
        {
            get { return _maxNum; }
            set
            {
                if (value >= MinNum && value.ToString("D").Length <= NumLength)
                {
                    _maxNum = value;
                    OnPropertyChanged();

                    if (CurrNum > _maxNum)
                        CurrNum = _maxNum;
                }
            }
        }

        private ulong _currNum;
        public ulong CurrNum
        {
            get { return _currNum; }
            set
            {
                if (value >= MinNum && value <= MaxNum)
                {
                    _currNum = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint _increment = 1;
        public uint Increment { get { return _increment; } set { _increment = value; OnPropertyChanged(); } }

        #endregion Properties

        #region Rule Common

        public IRuleObject Clone
        {
            get
            {
                var obj = new RuleSequentialNum();
                obj.NumLength = NumLength;
                obj.MaxNum = MaxNum;
                obj.MinNum = MinNum;
                obj.CurrNum = CurrNum;
                obj.Increment = Increment;

                return obj;
            }
        }

        public string PrintValue => CurrNum.ToString("D" + NumLength.ToString());

        #endregion Rule Common
    }
}
