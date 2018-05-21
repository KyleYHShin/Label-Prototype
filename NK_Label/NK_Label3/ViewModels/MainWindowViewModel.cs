using BasicModule.Common;
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
using NK_Label.Models;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using NK_Label.Utils;
using System.Collections.Generic;
using BasicModule;

namespace NK_Label.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {

        #region System Properties
        
        public string Title { get { return SystemInfo.Name + "  (" + SystemInfo.Version + ")"; } }

        private SystemLanguage _language;
        public SystemLanguage Language { get { return _language; } set { _language = value; OnPropertyChanged(); } }

        #endregion System Properties

        #region Tab Contents

        private ObservableCollection<LabelView> _labelViewList;
        public ObservableCollection<LabelView> LabelViewList
        {
            get { return _labelViewList; }
            set
            {
                _labelViewList = value;
                OnPropertyChanged();
                UsingLabelList.LabelViewList = value;
            }
        }

        private LabelView _selectedLabelView;
        public LabelView SelectedLabelView
        {
            get { return _selectedLabelView; }
            set
            {
                _selectedLabelView = value;
                OnPropertyChanged();
                UsingLabelList.SelectedLabelView = value;

                if (value is LabelView)
                {
                    (value.DataContext as LabelViewModel).ChangeOptionRegion();
                    HasLabel = true;
                }
                else
                    HasLabel = false;
            }
        }

        private bool _hasLabel;
        public bool HasLabel { get { return _hasLabel; } set { _hasLabel = value; OnPropertyChanged(); } }

        #endregion Tab Contents

        #region Constructor

        private readonly IRegionManager RegionManager;
        public MainWindowViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;

            //Load System Properties (system.ini)
            //Load Theme (\Theme 폴더\Theme.xml) 
            //Load SystemLanguage  (\Language 폴더\Korean.ini) 
            Language = new SystemLanguage();

            LabelViewList = new ObservableCollection<LabelView>();

            ClickAddNewLabel = new DelegateCommand(AddNewLabel);
            ClickOpen = new DelegateCommand(Open);
            ClickSave = new DelegateCommand(Save);
            ClickSaveAs = new DelegateCommand(SaveAs);
            ClickSaveAll = new DelegateCommand(SaveAll);
            ClickPrintCurrentLabel = new DelegateCommand(PrintCurrentLabel);
            ClickCloseCurrentLabel = new DelegateCommand(CloseCurrentLabel);
            ClickCloseAllLabel = new DelegateCommand(CloseAllLabel);
            ClickCloseWindow = new DelegateCommand(CloseWindow);
            //ClickCloseWindow = new DelegateCommand(Application.Current.Shutdown);
            //Application.Current.MainWindow.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            ClickAddText = new DelegateCommand(AddTextObject);
            ClickAddBarcode = new DelegateCommand(AddBarcodeObject);
            ClickDeleteObject = new DelegateCommand(DeleteObject);
            ClickEidtRuleList = new DelegateCommand(EditRuleList);

            ClickShowVersion = new DelegateCommand(ShowVersion);
        }

        #endregion Constructor

        #region Label Window Control Events

        public ICommand ClickCloseWindow { get; private set; }
        private void CloseWindow()
        {
            if (CanCloseAllLabel())
                Application.Current.Shutdown();
        }

        public ICommand ClickCloseCurrentLabel { get; private set; }
        private void CloseCurrentLabel()
        {
            CanCloseCurrentLabel();
        }

        public ICommand ClickCloseAllLabel { get; private set; }
        private void CloseAllLabel()
        {
            CanCloseAllLabel();
        }

        private bool CanCloseCurrentLabel()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var lvm = SelectedLabelView.DataContext as LabelViewModel;
                if (lvm.IsChanged)
                {
                    if (DialogService.ShowSimpleSelectDialog(Application.Current.MainWindow, "Warning", "'" + lvm.Label.Name + "'에 수정된 항목이 있습니다.\n 무시하고 종료하시겠습니까?") == true)
                    {
                        CloseLabel();
                        return true;
                    }
                }
                else
                {
                    CloseLabel();
                    return true;
                }
            }
            return false;
        }
        public bool CanCloseAllLabel()
        {
            while (LabelViewList.Count > 0 && SelectedLabelView != null)
            {
                if (!CanCloseCurrentLabel())
                    return false;
            }
            return true;
        }

        private void CloseLabel()
        {
            RegionManager.Regions[RegionNames.OptionRegion].RemoveAll();
            LabelViewList.Remove(SelectedLabelView);
            if (LabelViewList.Count > 0)
                SelectedLabelView = LabelViewList[LabelViewList.Count - 1];
            else
                SelectedLabelView = null;
        }

        #endregion Label Window Control Events

        #region Label Object Events

        private void AddLabel(LabelViewModel newLVM)
        {
            if (!CheckLicense())
                return;

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
            if (!CheckLicense())
                return;

            List<string> labelNames = new List<string>();
            foreach (var view in LabelViewList)
                    labelNames.Add((view.DataContext as LabelViewModel).Label.Name);

            var newLabel = new LabelObject();
            var olViewModel = new OptionLabelViewModel(newLabel, true);
            var olView = new OptionLabelView();
            olView.DataContext = olViewModel;

            if (DialogService.ShowSelectDialog(Application.Current.MainWindow, olView, "Create New Label") == true)
            {
                var newLVM = new LabelViewModel(RegionManager);
                newLVM.Label = newLabel;
                AddLabel(newLVM);
            }
        }

        public ICommand ClickAddText { get; private set; }
        private void AddTextObject()
        {
            if (!CheckLicense())
                return;

            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var newText = new TextObject();
                newText.Name = "NewText";

                var optionViewModel = new OptionTextViewModel(newText, true);
                var optionView = new OptionTextView();
                optionView.DataContext = optionViewModel;

                if (DialogService.ShowSelectDialog(Application.Current.MainWindow, optionView, "Create New Text") == true)
                {
                    var thisViewModel = SelectedLabelView.DataContext as LabelViewModel;
                    thisViewModel.ObjectList.Add(newText);
                    newText.IsChanged = true;

                    foreach (var obj in thisViewModel.ObjectList)
                        obj.IsSelected = false;

                    newText.IsSelected = true;
                }
            }
        }

        public ICommand ClickAddBarcode { get; private set; }
        private void AddBarcodeObject()
        {
            if (!CheckLicense())
                return;

            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var newBarcode = new BarcodeObject();
                newBarcode.Name = "NewBarcode";

                var optionViewModel = new OptionBarcodeViewModel(newBarcode, true);
                var optionView = new OptionBarcodeView();
                optionView.DataContext = optionViewModel;

                if (DialogService.ShowSelectDialog(Application.Current.MainWindow, optionView, "Create New Barcode") == true)
                {
                    var thisViewModel = SelectedLabelView.DataContext as LabelViewModel;
                    thisViewModel.ObjectList.Add(newBarcode);
                    newBarcode.IsChanged = true;

                    foreach (var obj in thisViewModel.ObjectList)
                        obj.IsSelected = false;

                    newBarcode.IsSelected = true;
                }
            }
        }

        public ICommand ClickDeleteObject { get; private set; }
        private void DeleteObject()
        {
            if (!CheckLicense())
                return;

            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
                (SelectedLabelView.DataContext as LabelViewModel).DeleteObject();
        }

        #endregion Label Object Events

        #region Rule Events

        public ICommand ClickEidtRuleList { get; private set; }
        private void EditRuleList()
        {
            if (!CheckLicense())
                return;

            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var LVM = SelectedLabelView.DataContext as LabelViewModel;

                var ruleManagerWin = new RuleManagerWindow(RegionManager);
                var rmWinVM = ruleManagerWin.DataContext as RuleManagerWindowViewModel;
                rmWinVM.RuleList = LVM.CloneObservableRuleList;

                ruleManagerWin.Owner = Application.Current.MainWindow;

                if (ruleManagerWin .ShowDialog()== true)
                {
                    LVM.RuleList.Clear();
                    foreach (var cr in rmWinVM.RuleList)
                        LVM.RuleList.Add(cr);
                }
            }
        }

        #endregion Rule Events

        #region File Control Events

        public ICommand ClickOpen { get; private set; }
        private void Open()
        {
            if (!CheckLicense())
                return;

            LabelViewModel newLVM = FileController.OpenLabel(RegionManager);
            if (newLVM != null)
            {
                var newPath = newLVM.FilePath;
                bool isExist = false;
                foreach (LabelView lv in LabelViewList)
                {
                    var lvm = lv.DataContext as LabelViewModel;
                    if (!string.IsNullOrEmpty(lvm.FilePath) && lvm.FilePath.Equals(newPath))
                    {
                        DialogService.ShowSimpleTextDialog("Warning", "해당 라벨 디자인 파일이 이미 열려있습니다.");
                        isExist = true;
                        break;
                    }
                }
                if (!isExist && UsingLabelList.UsingLabelNameList.Contains(newLVM.Label.Name))
                {
                    DialogService.ShowSimpleTextDialog("Warning", "해당 라벨의 이름(" + newLVM.Label.Name + ")이 이미 사용중입니다.");
                    isExist = true;
                }
                if (!isExist)
                    AddLabel(newLVM);
            }
        }
        
        public ICommand ClickSave { get; private set; }
        private void Save()
        {
            if (!CheckLicense())
                return;

            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var labelVM = SelectedLabelView.DataContext as LabelViewModel;
                FileController.SaveLabel(ref labelVM, false);
                labelVM.IsChanged = false;
            }
        }

        public ICommand ClickSaveAll { get; private set; }
        private void SaveAll()
        {
            if (!CheckLicense())
                return;

            foreach(var view in LabelViewList)
            {
                var labelVM = view.DataContext as LabelViewModel;
                FileController.SaveLabel(ref labelVM, false);
                labelVM.IsChanged = false;
            }
        }

        public ICommand ClickSaveAs { get; private set; }
        private void SaveAs()
        {
            if (!CheckLicense())
                return;

            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var labelVM = SelectedLabelView.DataContext as LabelViewModel;
                FileController.SaveLabel(ref labelVM, true);
                labelVM.IsChanged = false;
            }
        }

        #endregion File Control Events

        public ICommand ClickPrintCurrentLabel { get; private set; }
        private void PrintCurrentLabel()
        {
            if (!CheckLicense())
                return;

            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var pWin = new PrintWindow(RegionManager) { Title = "Print Label" };

                var LVM = SelectedLabelView.DataContext as LabelViewModel;
                var pVM = pWin.DataContext as PrintWindowViewModel;
                if (pVM.Initialize(LVM.Label, LVM.ObjectList, LVM.RuleList))
                {
                    var pLV = new PrintLabelView() { DataContext = pVM };
                    pWin.SetPrintLabelView(pLV);

                    pWin.Owner = Application.Current.MainWindow;
                    pWin.ShowDialog();
                }
                else
                    DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "Warning", "2개 이상의 Sequential Number 규칙이 적용되었습니다.");

            }
        }

        public ICommand ClickShowVersion { get; private set; }
        private void ShowVersion()
        {
            string msg = "Product Name: \t" + SystemInfo.Name;
            msg += "\nVersion : \t" + SystemInfo.Version;
            msg += "\nRelease  Date: \t" + SystemInfo.ReleaseDate.ToString("yyyy.MM.dd");
            msg += "\nService Expiration Date: \t" + Namkang.License.Controller.ProgramLicense.ServiceExpirationDate.ToString("yyyy.MM.dd");
            msg += "\n\nDeveloped by NAMKANG HI-TECH CO., LTD.";
            DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, Language.MenuHelpProgInfo, msg);
        }

        private bool CheckLicense()
        {
            string hardLockLoginErrMsg = Namkang.License.Controller.Login();

            if (string.IsNullOrEmpty(hardLockLoginErrMsg))
                return true;
            else
            {
                DialogService.ShowSimpleTextDialog("Warning", hardLockLoginErrMsg);
                return false;
            }
        }

    }
}
