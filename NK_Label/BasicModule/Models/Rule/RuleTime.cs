using BasicModule.Models.Common;
using BasicModule.Utils;

namespace BasicModule.Models.Rule
{
    public class RuleTime : NotifyPropertyChanged, IRuleObject
    {
        #region Properties

        private string _pattern = "yyyy-MM-dd";
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

        #endregion

        public IRuleObject Clone
        {
            get
            {
                var obj = new RuleTime();
                obj.Pattern = Pattern;

                return obj;
            }
        }

        public string PrintValue => TimeConversion.DateToString(Pattern);

    }
}
