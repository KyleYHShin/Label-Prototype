using BasicModule.Models.Common;

namespace BasicModule.Models.Rule
{
    public class RuleMain : NotifyPropertyChanged, IRuleObject
    {
        #region Properties

        private RuleRegulation.RuleFormat _format;
        public RuleRegulation.RuleFormat Format
        {
            get { return _format; }
            set
            {
                _format = value;
                OnPropertyChanged();

                if (_format == RuleRegulation.RuleFormat.TIME && !(Content is RuleTime))
                    Content = new RuleTime();
                else if (_format == RuleRegulation.RuleFormat.SEQUENTIAL_NUM && !(Content is RuleSequentialNum))
                    Content = new RuleSequentialNum();
                else if (_format == RuleRegulation.RuleFormat.MANUAL_LIST && !(Content is RuleManualList))
                    Content = new RuleManualList();
            }
        }

        private string _name; // Prefix && Postfix 미포함
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length >= RuleRegulation.MIN_NAME_LEN)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description { get { return _description; } set { _description = value; OnPropertyChanged(); } }

        private IRuleObject _content;
        public IRuleObject Content { get { return _content; } set { _content = value; OnPropertyChanged(); } }

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
