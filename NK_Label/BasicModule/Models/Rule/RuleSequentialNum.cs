namespace BasicModule.Models.Rule
{
    public class RuleSequentialNum : INotifyProperty, IRuleObject
    {
        private int _numLength = 2;
        public int NumLength
        {
            get { return _numLength; }
            set
            {
                if (value > 0 && value <= 10)
                {
                    _numLength = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _minNum;
        public int MinNum
        {
            get
            {
                if (_minNum > _maxNum)
                    _minNum = _maxNum;
                return _minNum;
            }
            set
            {
                if (value >= 0 && value <= _maxNum && value.ToString("D").Length <= NumLength)
                {
                    _minNum = value;
                    if (CurrNum < _minNum)
                        CurrNum = _minNum;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxNum;
        public int MaxNum
        {
            get
            {
                if (_maxNum < _minNum)
                    _maxNum = _minNum;
                return _maxNum;
            }
            set
            {
                if (value >= 0 && value >= _minNum && value.ToString("D").Length <= NumLength)
                {
                    _maxNum = value;
                    if (_currNum > _maxNum)
                        _currNum = _maxNum;
                    OnPropertyChanged();
                }
            }
        }

        private int _currNum;
        public int CurrNum
        {
            get { return _currNum; }
            set
            {
                if (value >= _minNum && value <= _maxNum)
                {
                    _currNum = value;
                    OnPropertyChanged();
                }
            }
        }
        public string CurrNumStr
        {
            get
            {
                return _currNum.ToString("D" + _numLength.ToString());
            }
        }

        private int _increment = 1;
        public int Increment
        {
            get { return _increment; }
            set
            {
                if (value > 0)
                {
                    _increment = value;
                    OnPropertyChanged();
                }
            }
        }


        public IRuleObject Clone()
        {
            RuleSequentialNum rsn = new RuleSequentialNum();
            rsn.NumLength = NumLength;
            rsn.MinNum = MinNum;
            rsn.MaxNum = MaxNum;
            rsn.CurrNum = CurrNum;
            rsn.Increment = Increment;

            return rsn;
        }

        public string PrintValue()
        {
            var str = CurrNumStr;
            CurrNum += Increment;
            return str;
        }
    }
}
