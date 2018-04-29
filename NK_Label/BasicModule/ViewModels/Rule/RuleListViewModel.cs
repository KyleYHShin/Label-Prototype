using BasicModule.Models.Rule;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;

namespace BasicModule.ViewModels.Rule
{
    public class RuleListViewModel : BindableBase
    {
        private List<RuleMain> _ruleList;
        public List<RuleMain> RuleList { get { return _ruleList; } set { SetProperty(ref _ruleList, value); } }

        private RuleMain _selectedRule;
        public RuleMain SelectedRule {
            get { return _selectedRule; }
            set 
            {
                SetProperty(ref _selectedRule, value);

                if (_selectedRule == null)
                    return;
                new RuleEditorViewModel(RegionManager, RuleList, _selectedRule);
            }
        }

        private readonly IRegionManager RegionManager;
        public RuleListViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }
        
    }
}
