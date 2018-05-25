using BasicModule.Common;
using System.Collections.ObjectModel;
using System.Text;

namespace BasicModule.Models.Rule.Content
{
    public class RuleInputCombine : NotifyPropertyChanged, IRuleObject
    {
        #region Properties

        private string _seperator = ",";
        public string Seperator
        {
            get { return _seperator; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _seperator = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private ObservableCollection<string> _inputList;
        public ObservableCollection<string> InputList { get { return _inputList; } set { _inputList = value; OnPropertyChanged(); } }

        #endregion Properties

        public void AddInput(string str)
        {
            if (InputList == null)
                InputList = new ObservableCollection<string>();

            string convertedStr = string.Empty;

            if (StartIndex <= 0 || CharLength <= 0 || StartIndex > str.Length)
                convertedStr = string.Empty;
            else if (StartIndex + CharLength - 1 >= str.Length)
                convertedStr= str.Substring(_startIndex);
            else
                convertedStr = str.Substring(_startIndex, CharLength);

            InputList.Add(convertedStr);
        }

        public void InputRefresh()
        {
            InputList = new ObservableCollection<string>();
        }

        #region Rule Common

        public IRuleObject Clone
        {
            get
            {
                RuleInputCombine obj = new RuleInputCombine();
                obj.InputList = InputList;
                obj.Seperator = Seperator;
                obj.StartIndex = StartIndex;
                obj.CharLength = CharLength;

                return obj;
            }
        }

        public string PrintValue
        {
            get
            {
                if (InputList == null)
                    return string.Empty;

                StringBuilder sumOfInputs = new StringBuilder();
                foreach (var str in InputList)
                {
                    if (sumOfInputs.Length > 0)
                        sumOfInputs.Append(Seperator);
                    sumOfInputs.Append(str);
                }

                return sumOfInputs.ToString();
            }
        }

        #endregion Rule Common
    }
}
