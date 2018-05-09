using BasicModule.Models.Common;

namespace BasicModule.Models.Rule
{
    public class RuleInput : NotifyPropertyChanged, IRuleObject
    {
        private int _order = 1;
        public int Order { get { return _order; } set { _order = value; OnPropertyChanged(); } }

        private int _startIndex;
        public int StartIndex { get { return _startIndex + 1; } set { _startIndex = value - 1; OnPropertyChanged(); } }

        private int _length = 1;
        public int Length { get { return _length; } set { _length = value; OnPropertyChanged(); } }

        private string _inputData = "";
        public string InputData { get { return _inputData; } set { _inputData = value; OnPropertyChanged(); } }

        public void InputRefresh()
        {
            InputData = string.Empty;
        }

        public IRuleObject Clone
        {
            get {
                var obj = new RuleInput();
                obj.Order = Order;
                obj.StartIndex = StartIndex;
                obj.Length = Length;
                obj.InputData = InputData;

                return obj;
            }
        }

        public string PrintValue
        {
            get
            {
                if (StartIndex<= 0 || Length <= 0 || StartIndex > InputData.Length)
                    return "";

                if (StartIndex + Length-1 >= InputData.Length)
                    return InputData.Substring(_startIndex);
                
                return InputData.Substring(_startIndex, Length);
            }
        }

    }
}
