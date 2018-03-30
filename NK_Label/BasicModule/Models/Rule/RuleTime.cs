using BasicModule.Utils;

namespace BasicModule.Models.Rule
{
    public class RuleTime : INotifyProperty, IRuleObject
    {
        private string _pattern;
        public string Pattern
        {
            get { return _pattern; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    _pattern = value;
                    OnPropertyChanged();
                }
            }
        }

        public IRuleObject Clone()
        {
            RuleTime rt = new RuleTime();
            rt.Pattern = Pattern;

            return rt;
        }

        public string PrintValue()
        {
            return TimeConversion.DateToString(Pattern);
        }

    }
}
