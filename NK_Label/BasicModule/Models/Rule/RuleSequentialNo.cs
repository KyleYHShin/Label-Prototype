namespace BasicModule.Models.Rule
{
    public class RuleSequentialNo : INotifyProperty
    {
        //총길이 default 2자리

        private int _minNo;
        public int MinNo
        {
            get { return _minNo; }
            set
            {
                if (value >= 0 && value < _maxNo) //자리수 적용
                {
                    _minNo = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxNo;
        public int MaxNo
        {
            get
            {
                if (_maxNo < _minNo)
                    _maxNo = _minNo;
                return _maxNo;
            }
            set
            {
                if (value >= 0 && value >= _minNo) //자리수 적용
                {
                    _maxNo = value;
                    OnPropertyChanged();
                }
            }
        }
        //현재
        //증가수





    }
}
