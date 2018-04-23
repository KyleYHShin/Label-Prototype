using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using BasicModule.Files;
using BasicModule.Models;
using BasicModule.Utils;
using BasicModule.Views;
using BasicModule.Views.Option;
using BasicModule.Views.Print;
using BasicModule.Views.Rule;

using BasicModule.ViewModels;
using BasicModule.ViewModels.Option;
using BasicModule.ViewModels.Print;
using BasicModule.ViewModels.Rule;

using NK_Label3.Models;

namespace NK_Label3.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        #region System Properties

        private string _title = "NK-Label 1.0.0";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private double _width = 1280;
        private double _height = 720;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        private string _background = "#FFDEDEDE";
        public string Background
        {
            get { return _background; }
            set { SetProperty(ref _background, value); }
        }

        private SystemLanguage _language;
        public SystemLanguage Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }
        }

        #endregion //System Properties

        private IDialogService DialogService { get { return new DialogService(); } }

        #region Tab Contents

        private ObservableCollection<LabelView> _labelViewList;
        public ObservableCollection<LabelView> LabelViewList
        {
            get { return _labelViewList; }
            set { SetProperty(ref _labelViewList, value); }
        }

        private LabelView _selectedLabelView;
        public LabelView SelectedLabelView
        {
            get { return _selectedLabelView; }
            set
            {
                SetProperty(ref _selectedLabelView, value);
                if (value is LabelView)
                    (value.DataContext as LabelViewModel).ChangeOptionRegion();
            }
        }

        #endregion // Tab Contents

        #region Constructor

        private readonly IRegionManager _regionManager;
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            Language = new SystemLanguage();
            //language.Load();
            LabelViewList = new ObservableCollection<LabelView>();

            ClickAddNewLabel = new DelegateCommand(AddNewLabel);
            ClickCloseCurrentLabel = new DelegateCommand(CloseCurrentLabel);
            ClickCloseAllLabel = new DelegateCommand(CloseAllLabel);

            ClickAddText = new DelegateCommand(AddTextObject);
            ClickAddBarcode = new DelegateCommand(AddBarcodeObject);
            ClickDeleteObject = new DelegateCommand(DeleteObject);//Test
            ClickEidtRuleList = new DelegateCommand(EditRuleList);

            ClickOpen = new DelegateCommand(Open);
            ClickSave = new DelegateCommand(Save);
            ClickSaveAs = new DelegateCommand(SaveAs);
            ClickPrintCurrentLabel = new DelegateCommand(PrintCurrentLabel);
        }

        #endregion Constructor

        #region Label Control Events

        private void AddLabel(LabelViewModel newLVM)
        {
            if (newLVM != null)
            {
                var newView = new LabelView();
                newView.DataContext = newLVM;
                LabelViewList.Add(newView);
                SelectedLabelView = newView;
            }
        }

        public ICommand ClickAddNewLabel { get; private set; }
        private void AddNewLabel()
        {
            var newLabel = new LabelObject();
            var olViewModel = new OptionLabelViewModel(newLabel); ;
            var olView = new OptionLabelView();
            olView.DataContext = olViewModel;

            if (DialogService.ShowSelectDialog(Application.Current.MainWindow, olView, "Create New Label") == true)
            {
                var newLVM = new LabelViewModel(_regionManager);
                newLVM.Label = newLabel;
                AddLabel(newLVM);
            }
        }

        private bool CanCloseCurrentLabel()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var lvm = SelectedLabelView.DataContext as LabelViewModel;
                if (lvm != null && lvm.IsChanged)
                {
                    if (DialogService.ShowSimpleSelectDialog(Application.Current.MainWindow, "Warning", "'" + lvm.Label.Name + "'에 수정된 항목이 있습니다.\n 무시하고 종료하시겠습니까?") == true)
                    {
                        _regionManager.Regions["OptionRegion"].RemoveAll();
                        LabelViewList.Remove(SelectedLabelView);
                        return true;
                    }
                }
                else
                {
                    _regionManager.Regions["OptionRegion"].RemoveAll();
                    LabelViewList.Remove(SelectedLabelView);
                    return true;
                }
            }
            return false;
        }

        public ICommand ClickCloseCurrentLabel { get; private set; }
        private void CloseCurrentLabel()
        {
            CanCloseCurrentLabel();
        }

        public ICommand ClickCloseAllLabel { get; private set; }
        private void CloseAllLabel()
        {
            while (LabelViewList.Count > 0 && SelectedLabelView != null)
            {
                if (!CanCloseCurrentLabel())
                    break;
            }
        }

        #endregion Label Control Events

        #region Label Object Events

        public ICommand ClickAddText { get; private set; }
        private void AddTextObject()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var newText = new TextObject();
                newText.Name = "New Text";

                var optionViewModel = new OptionTextViewModel(newText); ;
                var optionView = new OptionTextView();
                optionView.DataContext = optionViewModel;

                if (DialogService.ShowSelectDialog(Application.Current.MainWindow, optionView, "Create New Text") == true)
                {
                    var thisViewModel = SelectedLabelView.DataContext as LabelViewModel;
                    thisViewModel.ObjectList.Add(newText);
                    newText.Changed = true;
                }
            }
        }

        public ICommand ClickAddBarcode { get; private set; }
        private void AddBarcodeObject()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var newBarcode = new BarcodeObject();
                newBarcode.Name = "New Barcode";

                var optionViewModel = new OptionBarcodeViewModel(newBarcode); ;
                var optionView = new OptionBarcodeView();
                optionView.DataContext = optionViewModel;

                if (DialogService.ShowSelectDialog(Application.Current.MainWindow, optionView, "Create New Barcode") == true)
                {
                    var thisViewModel = SelectedLabelView.DataContext as LabelViewModel;
                    thisViewModel.ObjectList.Add(newBarcode);
                    newBarcode.Changed = true;
                }
            }
        }

        public ICommand ClickDeleteObject { get; private set; }
        private void DeleteObject()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
                (SelectedLabelView.DataContext as LabelViewModel).DeleteObject();
        }
        #endregion // Label Object Events

        #region Rule Events

        public ICommand ClickEidtRuleList { get; private set; }
        private void EditRuleList()
        {
            if(SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var LVM = SelectedLabelView.DataContext as LabelViewModel;
                var CopiedRuleList = LVM.CloneRuleList;
                var ruleEditViewModel = new RuleListViewModel(CopiedRuleList);
                var ruleEditView = new RuleListView();
                ruleEditView.DataContext = ruleEditViewModel;

                if (DialogService.ShowSelectDialog(Application.Current.MainWindow, ruleEditView, "Edit Rules") == true)
                {
                    LVM.RuleList = CopiedRuleList;
                }
            }
        }

        #endregion //Rule Events

        #region File Control Events

        public ICommand ClickOpen { get; private set; }
        private void Open()
        {
            LabelViewModel newLVM = FileController.OpenLabel(_regionManager);
            if (newLVM != null)
            {
                var newPath = newLVM.FilePath;
                bool isExist = false;
                foreach (LabelView lv in LabelViewList)
                {
                    var lvm = lv.DataContext as LabelViewModel;
                    if (lvm.FilePath.Equals(newPath))
                    {
                        MessageBox.Show("'" + lvm.Label.Name + "' 라벨이 이미 열려있습니다.");
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                    AddLabel(newLVM);
            }
        }

        public ICommand ClickSave { get; private set; }
        private void Save()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var labelVM = SelectedLabelView.DataContext as LabelViewModel;
                FileController.SaveLabel(ref labelVM, false);
                labelVM.IsChanged = false;
            }
        }
        public ICommand ClickSaveAs { get; private set; }
        private void SaveAs()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var labelVM = SelectedLabelView.DataContext as LabelViewModel;
                FileController.SaveLabel(ref labelVM, true);
                labelVM.IsChanged = false;
            }
        }

        #endregion //File Control Events
                
        public ICommand ClickPrintCurrentLabel { get; private set; }
        private void PrintCurrentLabel()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var LVM = SelectedLabelView.DataContext as LabelViewModel;
                var PVM = new PrintViewModel(LVM.Label, LVM.ObjectList, LVM.RuleList);

                var pLV = new PrintLabelView()
                {
                    DataContext = PVM
                };

                var pWin = new PrintWindow(pLV)
                {
                    Title = "Print Label",
                    DataContext = PVM
                };

                pWin.Owner = Application.Current.MainWindow;
                pWin.ShowDialog();
            }
        }

    }
}
