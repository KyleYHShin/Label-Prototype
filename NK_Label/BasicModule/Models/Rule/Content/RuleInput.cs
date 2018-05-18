using BasicModule.Common;

namespace BasicModule.Models.Rule.Content
{
    public class RuleInput : NotifyPropertyChanged, IRuleObject
    {
        #region Properties

        private int _order = 1;
        public int Order
        {
            get { return _order; }
            set
            {
                if (value > 0)
                {
                    _order = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _useInPrinting = true;
        public bool UseInPrinting { get { return _useInPrinting; } set { _useInPrinting = value; OnPropertyChanged(); } }

        private int _startIndex;
        public int StartIndex
        {
            get { return _startIndex + 1; }
            set
            {
                if (value > 0)
                {
                    _startIndex = value - 1;
                    OnPropertyChanged();
                }
            }
        }

        private int _charLength = 10;
        public int CharLength
        {
            get { return _charLength; }
            set
            {
                if (value > 0)
                {
                    _charLength = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _inputData = string.Empty;
        public string InputData { get { return _inputData; } set { _inputData = value; OnPropertyChanged(); } }

        #endregion Properties

        public void InputRefresh()
        {
            InputData = string.Empty;
        }

        #region Rule Common

        public IRuleObject Clone
        {
            get
            {
                var obj = new RuleInput();
                obj.Order = Order;
                obj.UseInPrinting = UseInPrinting;
                obj.StartIndex = StartIndex;
                obj.CharLength = CharLength;
                obj.InputData = InputData;

                return obj;
            }
        }

        public string PrintValue
        {
            get
            {
                if (StartIndex <= 0 || CharLength <= 0 || StartIndex > InputData.Length)
                    return string.Empty;

                if (StartIndex + CharLength - 1 >= InputData.Length)
                    return InputData.Substring(_startIndex);

                return InputData.Substring(_startIndex, CharLength);
            }
        }

        #endregion Rule Common
    }
}
