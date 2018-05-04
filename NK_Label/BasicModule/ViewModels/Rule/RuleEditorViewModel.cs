using BasicModule.Common;
using BasicModule.Models.Rule;
using BasicModule.Utils;
using BasicModule.Views.Rule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BasicModule.ViewModels.Rule
{
    public class RuleEditorViewModel : BindableBase
    {
        #region Rule Properties

        private readonly ObservableCollection<RuleMain> OriginalRuleList;
        private RuleMain OriginalRule;
        private readonly List<RuleInput> OtherRuleInputList;

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
                    case RuleRegulation.RuleFormat.INPUT:
                        var inputView = new RuleInputView();
                        inputView.DataContext = this;
                        RegionManager.Regions[RegionNames.RuleContent].Add(inputView, null, true);
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
        public RuleEditorViewModel(IRegionManager regionManager, ObservableCollection<RuleMain> originalRuleList, RuleMain originalRule)
        {
            RegionManager = regionManager;
            RegionManager.Regions[RegionNames.RuleCommon].RemoveAll();
            RegionManager.Regions[RegionNames.RuleContent].RemoveAll();
            RegionManager.Regions[RegionNames.RuleButton].RemoveAll();
            
            OriginalRuleList = originalRuleList;
            OriginalRule = originalRule;

            OtherRuleInputList = new List<RuleInput>();
            foreach(var r in OriginalRuleList)
            {
                if (r.Content is RuleInput && r != OriginalRule)
                    OtherRuleInputList.Add(r.Content as RuleInput);
            }

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
            IsEditMode = true;
        }

        public ICommand Delete { get; private set; }
        private void DeleteRule()
        {
            if (DialogService.ShowSimpleSelectDialog("Alarm", "'" + Rule.Name + "'을 삭제하시겠습니까?") == true)
            {
                OriginalRuleList.Remove(OriginalRule);
                foreach (var r in OriginalRuleList)
                    r.IsChanged = true;

                RegionManager.Regions[RegionNames.RuleCommon].RemoveAll();
                RegionManager.Regions[RegionNames.RuleContent].RemoveAll();
                RegionManager.Regions[RegionNames.RuleButton].RemoveAll();
            }
        }

        public ICommand Complete { get; private set; }
        private void EditComplete()
        {
            if (ErrorMsg != null)
            {
                DialogService.ShowSimpleTextDialog("Warning", ErrorMsg);
            }
            else
            {
                Rule.IsChanged = true;
                if (Rule.Content is RuleInput)
                    (Rule.Content as RuleInput).Refresh();
                //if (Rule.Content is RuleInputCombine)
                //    (Rule.Content as RuleInputCombine).Refresh();

                var index = OriginalRuleList.IndexOf(OriginalRule);
                OriginalRuleList.RemoveAt(index);
                OriginalRuleList.Insert(index, Rule);
                OriginalRule = Rule;
                Rule = OriginalRule.Clone as RuleMain; ;
                IsEditMode = false;
            }
        }
        private string ErrorMsg
        {
            get
            {
                if (string.IsNullOrEmpty(Rule.Name) || string.IsNullOrWhiteSpace(Rule.Name))
                    return "규칙 명이 비었습니다.";

                if (string.IsNullOrEmpty(Rule.Description) || string.IsNullOrWhiteSpace(Rule.Description))
                    return "규칙에 대한 설명이 비었습니다.";

                if (!RuleRegulation.BarcodeFormatList.ContainsValue(SelectedRuleFormat))
                    return "규칙 타입이 올바르지 않습니다.";

                string msg = CheckRuleNameDuplication();
                if (!string.IsNullOrEmpty(msg))
                    return msg;

                if (Rule.Content is RuleInput)
                    return CheckInputOrderDuplication();

                return null;
            }
        }

        private string CheckRuleNameDuplication()
        {
            foreach (var r in OriginalRuleList)
            {
                if (r.Equals(OriginalRule))
                    break;
                if (r.Name.Equals(Rule.Name))
                    return "중복된 규칙명이 존재합니다";
            }
            return null;
        }

        private string CheckInputOrderDuplication()
        {
            var newRule = Rule.Content as RuleInput;
            foreach (var r in OtherRuleInputList)
            {
                if (r.Order == newRule.Order)
                    return "중복된 순번이 존재합니다";
            }
            return null;
        }

        public ICommand Cancle { get; private set; }
        private void EditCancle()
        {
            Rule = OriginalRule.Clone as RuleMain;
            SelectedRuleFormat = Rule.Format;
            IsEditMode = false;
        }
    }
}
