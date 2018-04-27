using BasicModule.Models.Rule;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace BasicModule.ViewModels.Rule
{
    public class RuleEditorViewModel : BindableBase
    {
        #region Properties

        private RuleMain _newRule;
        public RuleMain Rule {
            get { return _newRule; }
            set { SetProperty(ref _newRule, value); }
        }

        public object RuleFormatList => RuleRegulation.BarcodeFormatList;
        public string TimeFormatList
        {
            get
            {
                string str = "";
                foreach (var pair in RuleRegulation.TimeFormatList)
                {
                    if (!string.IsNullOrEmpty(str))
                        str += "\n";
                    str += pair.Key + " : " + pair.Value;
                }
                return str;
            }
        }

        private bool _isEditMode = false;
        public bool IsEditMode { get { return _isEditMode; } set { SetProperty(ref _isEditMode, value); } }

        #endregion Properties

        public RuleEditorViewModel(RuleMain originalRule)
        {
            _newRule = originalRule.Clone as RuleMain;

            Edit = new DelegateCommand(ChagneToEditMode);
            Delete = new DelegateCommand(DeleteRule);

            OK = new DelegateCommand(ChangeToEditedRule);
            Cancle = new DelegateCommand(ReturnToOriginalRule);
        }
        
        public ICommand Edit { get; private set; }
        private void ChagneToEditMode()
        {
            IsEditMode = true;
        }

        public ICommand Delete { get; private set; }
        private void DeleteRule()
        {
            IsEditMode = false;
        }

        public ICommand OK { get; private set; }
        private void ChangeToEditedRule()
        {
            IsEditMode = false;
        }

        public ICommand Cancle { get; private set; }
        private void ReturnToOriginalRule()
        {
            IsEditMode = false;
        }
    }
}
