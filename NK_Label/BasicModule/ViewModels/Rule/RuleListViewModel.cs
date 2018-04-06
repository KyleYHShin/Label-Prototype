using BasicModule.Models.Rule;
using Prism.Mvvm;
using System.Collections.Generic;

namespace BasicModule.ViewModels.Rule
{
    public class RuleListViewModel : BindableBase
    {
        private List<RuleMain> _ruleList;
        public List<RuleMain> RuleList
        {
            get { return _ruleList; }
            set { SetProperty(ref _ruleList, value); }
        }

        private RuleMain _selectedRule;
        public RuleMain SelectedRule
        {
            get { return _selectedRule; }
            set { SetProperty(ref _selectedRule, value); }
        }


        public RuleListViewModel(List<RuleMain> ruleList)
        {
            RuleList = ruleList;
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
