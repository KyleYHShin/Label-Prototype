using BasicModule.Common;
using BasicModule.Models.Rule;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BasicModule.ViewModels.Rule
{
    public class RuleListViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<RuleMain> _ruleList;
        public ObservableCollection<RuleMain> RuleList { get { return _ruleList; } set { _ruleList = value; OnPropertyChanged(); } }

        private RuleMain _selectedRule;
        public RuleMain SelectedRule
        {
            get { return _selectedRule; }
            set
            {
                _selectedRule = value; OnPropertyChanged();

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
            string newName = "NewRule";
            int addNum = 1;
            foreach (var r in RuleList)
            {
                if (r.Name.Equals(newName + "_" + addNum.ToString()))
                    addNum++;
            }
            newRule.Name = newName + "_" + addNum.ToString();
            newRule.Format = RuleRegulation.RuleFormat.SEQUENTIAL_NUM;

            new RuleEditorViewModel(RegionManager, RuleList, newRule)
            {
                IsEditMode = true
            };
        }
    }
}
