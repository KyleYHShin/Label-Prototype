using static BasicModule.Models.Rule.RuleRregulation;

namespace BasicModule.Models.Rule
{
    public class Rule : INotifyProperty, IRuleObject
    {
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

        private string _name; // Prefix && Postfix 미포함
        public string Name
        {
            get { return RuleNameCombiner(_name); }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length >= MIN_NAME_LEN)
                {
                    _name = value;
                    OnPropertyChanged();
                }
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

        private IRuleObject _content;
        public IRuleObject Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        public IRuleObject Clone()
        {
            Rule rule = new Rule();
            rule.Format = Format;
            rule.Name = Name;
            rule.Description = Description;
            rule.Content = Content;

            return rule;
        }

        public string PrintValue()
        {
            if (Content != null)
                return Content.PrintValue();
            return "";
        }
    }
}
