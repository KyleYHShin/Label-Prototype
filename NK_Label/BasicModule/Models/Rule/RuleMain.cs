using BasicModule.Models.Common;

namespace BasicModule.Models.Rule
{
    public class RuleMain : NotifyPropertyChanged, IRuleObject
    {
        #region Properties

        private RuleRregulation.RuleFormat _format;
        public RuleRregulation.RuleFormat Format
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
            get { return _name; }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length >= RuleRregulation.MIN_NAME_LEN)
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

        #endregion

        public IRuleObject Clone
        {
            get
            {
                var obj = new RuleMain();
                obj.Format = Format;
                obj.Name = Name;
                obj.Description = Description;
                obj.Content = Content.Clone;

                return obj;
            }
        }

        public string PrintValue
        {
            get
            {
                if (Content != null)
                    return Content.PrintValue;
                return "";
            }
        }
    }
}
