using BasicModule.Models.Rule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BasicModule.ViewModels.Rule
{
    public class RuleListViewModel : BindableBase
    {
        private ObservableCollection<RuleMain> _ruleList;
        public ObservableCollection<RuleMain> RuleList { get { return _ruleList; } set { SetProperty(ref _ruleList, value); } }

        private RuleMain _selectedRule;
        public RuleMain SelectedRule {
            get { return _selectedRule; }
            set 
            {
                SetProperty(ref _selectedRule, value);

                if (_selectedRule == null)
                    return;
                new RuleEditorViewModel(RegionManager, RuleList, SelectedRule);
            }
        }

        private readonly IRegionManager RegionManager;
        public RuleListViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;

            Create = new DelegateCommand(CreateNewRule);
        }

        public ICommand Create { get; private set; }
        private void CreateNewRule()
        {
            var newRule = new RuleMain();
            string newName = "New Rule ";
            int addNum = 1;
            foreach(var r in RuleList)
            {
                if(r.Name.Equals(newName + addNum.ToString()))
                {
                    addNum++;
                }
            }
            newRule.Name = newName + addNum.ToString();
            newRule.Format = RuleRegulation.RuleFormat.SEQUENTIAL_NUM;

            RuleList.Add(newRule);
            SetProperty(ref _selectedRule, newRule);

            var reVM = new RuleEditorViewModel(RegionManager, RuleList, SelectedRule);
            reVM.IsEditMode = true;
        }
    }
}
