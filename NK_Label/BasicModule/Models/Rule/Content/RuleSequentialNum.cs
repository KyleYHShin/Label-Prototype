using BasicModule.Common;

namespace BasicModule.Models.Rule.Content
{
    public class RuleSequentialNum : NotifyPropertyChanged, IRuleObject
    {
        #region Properties

        private int _numLength = 2;
        public int NumLength
        {
            get { return _numLength; }
            set
            {
                if (value > ulong.MinValue.ToString().Length && value <= ulong.MaxValue.ToString().Length
                    && value >= MinNum.ToString().Length && value >= MaxNum.ToString().Length)
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
                if (value >= ulong.MinValue && value <= MaxNum && value.ToString("D").Length <= NumLength)
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
                if (value >= MinNum && value <= ulong.MaxValue && value.ToString("D").Length <= NumLength)
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

        private ulong _increment = 1;
        public ulong Increment
        {
            get { return _increment; }
            set
            {
                if (value > ulong.MinValue && value < ulong.MaxValue)
                {
                    _increment = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public string PrintValue => CurrNum.ToString("D" + _numLength.ToString());

        #endregion Rule Common
    }
}
