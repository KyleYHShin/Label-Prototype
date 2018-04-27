using BasicModule.Common;
using BasicModule.Models.Rule;
using BasicModule.Views.Rule;
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
                ChangeSideView();
            }
        }

        private void ChangeSideView()
        {
            
            _regionManager.Regions[RegionNames.RuleCommon].RemoveAll();
            _regionManager.Regions[RegionNames.RuleContent].RemoveAll();
            _regionManager.Regions[RegionNames.RuleButton].RemoveAll();

            if (SelectedRule == null)
                return;

            var reVM = new RuleEditorViewModel(SelectedRule);

            //var topView = new RuleCommonView();
            var topView = new RuleEditView();
            topView.DataContext = reVM;
            _regionManager.Regions[RegionNames.RuleCommon].Add(topView, null, true);

            //ChangeRuleFormat(SelectedRule.Format);
            
            var botView = new EditOrDeleteButtonView();
            botView.DataContext = reVM;
            _regionManager.Regions[RegionNames.RuleButton].Add(botView, null, true);

        }

        public void ChangeRuleFormat(RuleRegulation.RuleFormat newFormat)
        {
            if (SelectedRule == null)
                return;

            var reVM = new RuleEditorViewModel(SelectedRule);
            switch (newFormat)
            {
                case RuleRegulation.RuleFormat.MANUAL_LIST:
                    var mlistView = new RuleManualListView();
                    mlistView.DataContext = reVM;
                    _regionManager.Regions[RegionNames.RuleContent].Add(mlistView, null, true);
                    break;
                case RuleRegulation.RuleFormat.SEQUENTIAL_NUM:
                    var seqView = new RuleSequentialNumView();
                    seqView.DataContext = reVM;
                    _regionManager.Regions[RegionNames.RuleContent].Add(seqView, null, true);
                    break;
                case RuleRegulation.RuleFormat.TIME:
                    var timeView = new RuleTimeView();
                    timeView.DataContext = reVM;
                    _regionManager.Regions[RegionNames.RuleContent].Add(timeView, null, true);
                    break;
            }
        }

        private readonly IRegionManager _regionManager;
        public RuleListViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }


        public bool RuleNameDuplicationVerifier(RuleMain rule)
        {
            foreach (var r in RuleList)
                if (r.Name.Equals(rule.Name))
                    return false;

            return true;
        }
        
    }
}
