using BasicModule.Common;
using BasicModule.Models.Rule;
using BasicModule.Views.Rule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BasicModule.ViewModels.Rule
{
    public class RuleEditorViewModel : BindableBase
    {
        #region Rule Properties

        private readonly List<RuleMain> RuleList;

        private RuleMain CopiedRule;
        private RuleMain _rule;
        public RuleMain Rule { get { return _rule; } set { SetProperty(ref _rule, value); } }

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

        private RuleRegulation.RuleFormat _selectedRuleFormat;
        public RuleRegulation.RuleFormat SelectedRuleFormat
        {
            get { return _selectedRuleFormat; }
            set
            {
                Rule.Format = value;
                SetProperty(ref _selectedRuleFormat, value);

                RegionManager.Regions[RegionNames.RuleContent].RemoveAll();
                switch (value)
                {
                    case RuleRegulation.RuleFormat.MANUAL_LIST:
                        var mlistView = new RuleManualListView();
                        mlistView.DataContext = this;
                        RegionManager.Regions[RegionNames.RuleContent].Add(mlistView, null, true);
                        break;
                    case RuleRegulation.RuleFormat.SEQUENTIAL_NUM:
                        var seqView = new RuleSequentialNumView();
                        seqView.DataContext = this;
                        RegionManager.Regions[RegionNames.RuleContent].Add(seqView, null, true);
                        break;
                    case RuleRegulation.RuleFormat.TIME:
                        var timeView = new RuleTimeView();
                        timeView.DataContext = this;
                        RegionManager.Regions[RegionNames.RuleContent].Add(timeView, null, true);
                        break;
                }

                Example = Rule.Content.PrintValue;
            }
        }

        // only for manual list
        private KeyValuePair<string, string> _selectedManualContent;
        public KeyValuePair<string, string> SelectedManualContent
        {
            get { return _selectedManualContent; }
            set
            {
                SetProperty(ref _selectedManualContent, value);
                EditKey = value.Key;
                EditValue = value.Value;
            }
        }

        private string _example;
        public string Example { get { return _example; } set { SetProperty(ref _example, value); } }

        #endregion Rule Properties

        #region Editor Properties

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                SetProperty(ref _isEditMode, value);

                RegionManager.Regions[RegionNames.RuleButton].RemoveAll();
                if (_isEditMode)
                {
                    var botView = new CompleteOrCancleButtonView();
                    botView.DataContext = this;
                    RegionManager.Regions[RegionNames.RuleButton].Add(botView, null, true);
                }
                else
                {
                    var botView = new EditOrDeleteButtonView();
                    botView.DataContext = this;
                    RegionManager.Regions[RegionNames.RuleButton].Add(botView, null, true);
                }
            }
        }

        #endregion Editor Properties


        private readonly IRegionManager RegionManager;
        public RuleEditorViewModel(IRegionManager regionManager, List<RuleMain> originalRuleList, RuleMain originalRule)
        {
            RegionManager = regionManager;
            RegionManager.Regions[RegionNames.RuleCommon].RemoveAll();
            RegionManager.Regions[RegionNames.RuleContent].RemoveAll();
            RegionManager.Regions[RegionNames.RuleButton].RemoveAll();

            RuleList = originalRuleList;
            Rule = originalRule.Clone as RuleMain;

            SelectedRuleFormat = Rule.Format;

            var topView = new RuleCommonView();
            topView.DataContext = this;
            RegionManager.Regions[RegionNames.RuleCommon].Add(topView, null, true);

            IsEditMode = false;

            PrintExample = new DelegateCommand(GetExample);

            AddToList = new DelegateCommand(AddToManualList);
            UpdateList = new DelegateCommand(UpdateManualList);
            RemoveFromList = new DelegateCommand(RemoveFromManualList);

            Edit = new DelegateCommand(IntoEditMode);
            Delete = new DelegateCommand(DeleteRule);

            Complete = new DelegateCommand(EditComplete);
            Cancle = new DelegateCommand(EditCancle);
        }

        public ICommand PrintExample { get; private set; }
        private void GetExample()
        {
            Example = Rule.Content.PrintValue;
        }

        private string _editKey;
        public string EditKey { get { return _editKey; } set { SetProperty(ref _editKey, value); } }
        private string _editValue;
        public string EditValue { get { return _editValue; } set { SetProperty(ref _editValue, value); } }

        public ICommand AddToList { get; private set; }
        private void AddToManualList()
        {
            bool ret = false;
            if (Rule.Content is RuleManualList)
                ret = (Rule.Content as RuleManualList).AddList(EditKey, EditValue);
            if(ret)
            {
                EditKey = "";
                EditValue = "";
            }
        }
        public ICommand UpdateList { get; private set; }
        private void UpdateManualList()
        {
            bool ret = false;
            if (Rule.Content is RuleManualList)
                ret = (Rule.Content as RuleManualList).UpdateList(EditKey, EditValue);
            if (ret)
            {
                EditKey = "";
                EditValue = "";
            }
        }
        
        public ICommand RemoveFromList { get; private set; }
        private void RemoveFromManualList()
        {
            bool ret = false;
            if (Rule.Content is RuleManualList)
                ret = (Rule.Content as RuleManualList).RemoveList(EditKey);
            if (ret)
            {
                EditKey = "";
                EditValue = "";
            }
        }

        public ICommand Edit { get; private set; }
        private void IntoEditMode()
        {
            CopiedRule = Rule.Clone as RuleMain;
            IsEditMode = true;
        }

        public ICommand Delete { get; private set; }
        private void DeleteRule()
        {
            Console.WriteLine("Delete this rule");
        }

        public ICommand Complete { get; private set; }
        private void EditComplete()
        {

        }
        private bool Verifier()
        {
            foreach (var r in RuleList)
                if (r.Name.Equals(Rule.Name))
                    return false;
            // 기타 규칙 일치하는지 확인 후 리턴
            return true;
        }

        public ICommand Cancle { get; private set; }
        private void EditCancle()
        {
            Rule = CopiedRule;
            SelectedRuleFormat = Rule.Format;
            IsEditMode = false;
        }
    }
}
