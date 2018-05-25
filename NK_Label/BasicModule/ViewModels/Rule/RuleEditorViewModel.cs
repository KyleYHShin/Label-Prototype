using BasicModule.Common;
using BasicModule.Models.Rule;
using BasicModule.Models.Rule.Content;
using BasicModule.Utils;
using BasicModule.Views.Rule;
using BasicModule.Views.Rule.Content;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BasicModule.ViewModels.Rule
{
    public class RuleEditorViewModel : NotifyPropertyChanged
    {
        #region Rule Properties

        private readonly ObservableCollection<RuleMain> OriginalRuleList;
        private RuleMain OriginalRuleBeforeEdit;
        private readonly List<RuleInput> RuleInputListWithoutOriginalRule;

        private RuleMain _editingRule;
        public RuleMain EditingRule { get { return _editingRule; } set {  _editingRule = value; OnPropertyChanged(); } }

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
                EditingRule.Format = value;
                 _selectedRuleFormat = value; OnPropertyChanged();

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
                    case RuleRegulation.RuleFormat.INPUT_COMBINE:
                        var inputCombView = new RuleInputCombineView();
                        inputCombView.DataContext = this;
                        RegionManager.Regions[RegionNames.RuleContent].Add(inputCombView, null, true);
                        break;
                }

                Example = EditingRule.Content.PrintValue;
            }
        }

        // Only for manual list
        private KeyValuePair<string, string> _selectedManualContent;
        public KeyValuePair<string, string> SelectedManualContent
        {
            get { return _selectedManualContent; }
            set
            {
                 _selectedManualContent = value; OnPropertyChanged();
                EditKey = value.Key;
                EditValue = value.Value;
            }
        }

        private string _example;
        public string Example { get { return _example; } set {  _example = value; OnPropertyChanged(); } }

        #endregion Rule Properties

        #region Editor Properties

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                 _isEditMode = value; OnPropertyChanged();

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
            RegionManager.Regions[RegionNames.RuleButton].RemoveAll();

            // Initialize Rule Properties
            OriginalRuleList = originalRuleList;
            OriginalRuleBeforeEdit = originalRule;

            RuleInputListWithoutOriginalRule = new List<RuleInput>();
            foreach(var r in OriginalRuleList)
            {
                if (r.Content is RuleInput && r != OriginalRuleBeforeEdit)
                    RuleInputListWithoutOriginalRule.Add(r.Content as RuleInput);
            }

            EditingRule = originalRule.Clone as RuleMain;
            SelectedRuleFormat = EditingRule.Format;

            // Initialize View
            var topView = new RuleCommonView();
            topView.DataContext = this;
            RegionManager.Regions[RegionNames.RuleCommon].Add(topView, null, true);

            var botView = new EditOrDeleteButtonView();
            botView.DataContext = this;
            RegionManager.Regions[RegionNames.RuleButton].Add(botView, null, true);

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
            if (SelectedRuleFormat == RuleRegulation.RuleFormat.INPUT_COMBINE)
            {
                var content = EditingRule.Content as RuleInputCombine;
                content.InputRefresh();
                content.AddInput("Sample1");
                content.AddInput("Sample2");
                content.AddInput("Sample3");
            }
            Example = EditingRule.Content.PrintValue;
        }

        private string _editKey;
        public string EditKey { get { return _editKey; } set {  _editKey = value; OnPropertyChanged(); } }
        private string _editValue;
        public string EditValue { get { return _editValue; } set {  _editValue = value; OnPropertyChanged(); } }

        public ICommand AddToList { get; private set; }
        private void AddToManualList()
        {
            bool ret = false;
            if (EditingRule.Content is RuleManualList)
                ret = (EditingRule.Content as RuleManualList).AddList(EditKey, EditValue);
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
            if (EditingRule.Content is RuleManualList)
                ret = (EditingRule.Content as RuleManualList).UpdateList(EditKey, EditValue);
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
            if (EditingRule.Content is RuleManualList)
                ret = (EditingRule.Content as RuleManualList).RemoveList(EditKey);
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
            if (DialogService.ShowSimpleSelectDialog("Alarm", "'" + EditingRule.Name + "'을 삭제하시겠습니까?") == true)
            {
                OriginalRuleList.Remove(OriginalRuleBeforeEdit);
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
                var index = OriginalRuleList.Count;
                if (OriginalRuleList.Contains(OriginalRuleBeforeEdit))
                {
                    index = OriginalRuleList.IndexOf(OriginalRuleBeforeEdit);
                    OriginalRuleList.RemoveAt(index);
                }
                OriginalRuleList.Insert(index, EditingRule);
                OriginalRuleBeforeEdit = EditingRule;
                OriginalRuleBeforeEdit.IsChanged = true;

                EditingRule = OriginalRuleBeforeEdit.Clone as RuleMain; ;
                IsEditMode = false;
            }
        }

        private string ErrorMsg
        {
            get
            {
                if (string.IsNullOrEmpty(EditingRule.Name) || string.IsNullOrWhiteSpace(EditingRule.Name))
                    return "규칙 명이 비었습니다.";

                if (string.IsNullOrEmpty(EditingRule.Description) || string.IsNullOrWhiteSpace(EditingRule.Description))
                    return "규칙에 대한 설명이 비었습니다.";

                if (!RuleRegulation.BarcodeFormatList.ContainsKey(SelectedRuleFormat))
                    return "규칙 타입이 올바르지 않습니다.";

                string msg = CheckRuleNameDuplication();
                if (!string.IsNullOrEmpty(msg))
                    return msg;

                if (EditingRule.Content is RuleInput)
                    return CheckInputOrderDuplication();

                return null;
            }
        }

        private string CheckRuleNameDuplication()
        {
            foreach (var r in OriginalRuleList)
            {
                if (!r.Equals(OriginalRuleBeforeEdit) && r.Name.Equals(EditingRule.Name))
                    return "중복된 규칙명이 존재합니다";
            }
            return null;
        }

        private string CheckInputOrderDuplication()
        {
            var newRuleInput = EditingRule.Content as RuleInput;
            foreach (var r in RuleInputListWithoutOriginalRule)
            {
                if (r.Order == newRuleInput.Order)
                    return "중복된 순번이 존재합니다";
            }
            return null;
        }

        public ICommand Cancle { get; private set; }
        private void EditCancle()
        {
            EditingRule = OriginalRuleBeforeEdit.Clone as RuleMain;
            SelectedRuleFormat = EditingRule.Format;
            IsEditMode = false;
        }
    }
}
