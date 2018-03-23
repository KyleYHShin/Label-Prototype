using static BasicModule.Models.Rule.RuleRregulation;

namespace BasicModule.Models.Rule
{
    public class Rule : INotifyProperty
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private RuleFormat _format;
        public RuleFormat Format
        {
            get { return _format; }
            set
            {
                _format = value;
                OnPropertyChanged();
            }
        }

        private string _keyword;
        public string Keyword
        {
            get { return Prefix + _keyword + Postfix; }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length >= 2)
                {
                    _keyword = value;
                    OnPropertyChanged();
                }
            }
        }

        private object _ruleContent;
        public object RuleContent
        {
            get { return _ruleContent; }
            set
            {
                _ruleContent = value;
                OnPropertyChanged();
            }
        }

    }
}
