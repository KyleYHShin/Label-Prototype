using BasicModule.Common;
using BasicModule.Utils;

namespace BasicModule.Models.Rule.Content
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

        #endregion Properties

        #region Rule Common

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
        
        #endregion Rule Common
    }
}
