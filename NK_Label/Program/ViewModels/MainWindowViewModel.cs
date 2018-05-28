using BasicModule.Common;
using BasicModule.Files;
using BasicModule.Models;
using BasicModule.Utils;
using BasicModule.Views.Option;
using BasicModule.Views.Print;
using BasicModule.Views.Rule;
using BasicModule.ViewModels;
using BasicModule.ViewModels.Option;
using BasicModule.ViewModels.Print;
using BasicModule.ViewModels.Rule;
using Program.Models;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Program.Utils;
using BasicModule;

namespace Program.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {

        #region System Properties
        
        public string Title { get { return SystemInfo.Name; } }

        private SystemLanguage _language;
        public SystemLanguage Language { get { return _language; } set { _language = value; OnPropertyChanged(); } }

        #endregion System Properties

        #region Tab Contents

        private ObservableCollection<LabelViewModel> _labelList;
        public ObservableCollection<LabelViewModel> LabelList
        {
            get { return _labelList; }
            set
            {
                _labelList = value;
                OnPropertyChanged();
                UsingLabelList.LabelViewModelList = value;
            }
        }

        private LabelViewModel _selectedLabel;
        public LabelViewModel SelectedLabel
        {
            get { return _selectedLabel; }
            set
            {
                _selectedLabel = value;
                OnPropertyChanged();
                UsingLabelList.SelectedLabelViewModel = value;

                if (value != null)
                {
                    value.ChangeOptionRegion();
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

            LabelList = new ObservableCollection<LabelViewModel>();

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
            ClickRuleManager = new DelegateCommand(RuleManager);

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
            if (SelectedLabel != null)
            {
                if (SelectedLabel.IsChanged)
                {
                    if (DialogService.ShowSimpleSelectDialog(Application.Current.MainWindow, "확인", "'" + SelectedLabel.Label.Name + "'에 수정된 항목이 있습니다.\n 무시하고 종료하시겠습니까?") == true)
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
            while (LabelList.Count > 0 && SelectedLabel != null)
            {
                if (!CanCloseCurrentLabel())
                    return false;
            }
            return true;
        }

        private void CloseLabel()
        {
            RegionManager.Regions[RegionNames.OptionRegion].RemoveAll();
            LabelList.Remove(SelectedLabel);
            if (LabelList.Count > 0)
                SelectedLabel = LabelList[LabelList.Count - 1];
            else
                SelectedLabel = null;
        }

        #endregion Label Window Control Events

        #region Label Object Events

        private void AddLabel(LabelViewModel newLVM)
        {
            if (!HasLicense())
                return;

            if (newLVM != null)
            {
                LabelList.Add(newLVM);
                SelectedLabel = newLVM;
            }
        }

        public ICommand ClickAddNewLabel { get; private set; }
        private void AddNewLabel()
        {
            if (!HasLicense())
                return;

            var newLabel = new LabelObject();
            var olViewModel = new OptionLabelViewModel(newLabel, true);
            var olView = new OptionLabelView();
            olView.DataContext = olViewModel;

            if (DialogService.ShowSelectDialog(Application.Current.MainWindow, olView, "새 라벨") == true)
            {
                var newLVM = new LabelViewModel(RegionManager);
                newLVM.Label = newLabel;
                AddLabel(newLVM);
            }
        }

        public ICommand ClickAddText { get; private set; }
        private void AddTextObject()
        {
            if (!HasLicense())
                return;

            if (SelectedLabel != null)
            {
                var newText = new TextObject();
                newText.Name = "NewText";

                var optionViewModel = new OptionTextViewModel(newText, true);
                var optionView = new OptionTextView();
                optionView.DataContext = optionViewModel;

                if (DialogService.ShowSelectDialog(Application.Current.MainWindow, optionView, "새 텍스트") == true)
                {
                    SelectedLabel.ObjectList.Add(newText);
                    newText.IsChanged = true;

                    foreach (var obj in SelectedLabel.ObjectList)
                        obj.IsSelected = false;

                    newText.IsSelected = true;
                }
            }
        }

        public ICommand ClickAddBarcode { get; private set; }
        private void AddBarcodeObject()
        {
            if (!HasLicense())
                return;

            if (SelectedLabel != null)
            {
                var newBarcode = new BarcodeObject();
                newBarcode.Name = "NewBarcode";

                var optionViewModel = new OptionBarcodeViewModel(newBarcode, true);
                var optionView = new OptionBarcodeView();
                optionView.DataContext = optionViewModel;

                if (DialogService.ShowSelectDialog(Application.Current.MainWindow, optionView, "새 바코드") == true)
                {
                    SelectedLabel.ObjectList.Add(newBarcode);
                    newBarcode.IsChanged = true;

                    foreach (var obj in SelectedLabel.ObjectList)
                        obj.IsSelected = false;

                    newBarcode.IsSelected = true;
                }
            }
        }

        public ICommand ClickDeleteObject { get; private set; }
        private void DeleteObject()
        {
            if (!HasLicense())
                return;

            if (SelectedLabel != null)
                SelectedLabel.DeleteObject();
        }

        #endregion Label Object Events

        #region Rule Events
        
        public ICommand ClickRuleManager { get; private set; }
        private void RuleManager()
        {
            if (!HasLicense())
                return;

            if (SelectedLabel != null)
            {
                var ruleManagerWin = new RuleManagerWindow(RegionManager);
                var rmWinVM = ruleManagerWin.DataContext as RuleManagerWindowViewModel;
                rmWinVM.RuleList = SelectedLabel.CloneObservableRuleList;
                if (rmWinVM.RuleList.Count > 0)
                    rmWinVM.SelectedRule = rmWinVM.RuleList[0];

                ruleManagerWin.Owner = Application.Current.MainWindow;

                if (ruleManagerWin.ShowDialog() == true)
                {
                    SelectedLabel.RuleList.Clear();
                    foreach (var cr in rmWinVM.RuleList)
                        SelectedLabel.RuleList.Add(cr);
                }
            }
        }

        #endregion Rule Events

        #region File Control Events

        public ICommand ClickOpen { get; private set; }
        private void Open()
        {
            if (!HasLicense())
                return;

            LabelViewModel newLVM = FileController.OpenLabel(RegionManager);
            if (newLVM != null)
            {
                var newPath = newLVM.FilePath;
                bool isExist = false;
                foreach (LabelViewModel LVM in LabelList)
                {
                    if (!string.IsNullOrEmpty(LVM.FilePath) && LVM.FilePath.Equals(newPath))
                    {
                        DialogService.ShowSimpleTextDialog("확인", "해당 라벨 디자인 파일이 이미 열려있습니다.");
                        isExist = true;
                        break;
                    }
                }
                if (!isExist && UsingLabelList.UsingLabelNameList.Contains(newLVM.Label.Name))
                {
                    DialogService.ShowSimpleTextDialog("확인", "해당 라벨의 이름(" + newLVM.Label.Name + ")이 이미 사용중입니다.");
                    isExist = true;
                }
                if (!isExist)
                    AddLabel(newLVM);
            }
        }
        
        public ICommand ClickSave { get; private set; }
        private void Save()
        {
            if (SelectedLabel != null)
            {
                var LVM = SelectedLabel;
                FileController.SaveLabel(ref LVM, false);
                SelectedLabel.IsChanged = false;
            }
        }

        public ICommand ClickSaveAll { get; private set; }
        private void SaveAll()
        {
            foreach(var lvm in LabelList)
            {
                var LVM = lvm;
                FileController.SaveLabel(ref LVM, false);
                lvm.IsChanged = false;
            }
        }

        public ICommand ClickSaveAs { get; private set; }
        private void SaveAs()
        {
            if (SelectedLabel != null)
            {
                var LVM = SelectedLabel;
                FileController.SaveLabel(ref LVM, true);
                SelectedLabel.IsChanged = false;
            }
        }

        #endregion File Control Events

        public ICommand ClickPrintCurrentLabel { get; private set; }
        private void PrintCurrentLabel()
        {
            if (!HasLicense())
                return;

            if (SelectedLabel != null)
            {
                var pWin = new PrintWindow(RegionManager);
                var pVM = pWin.DataContext as PrintWindowViewModel;
                pVM.HasLicense = new PrintWindowViewModel.NKLicense(HasLicense);

                if (pVM.Initialize(SelectedLabel.Label, SelectedLabel.ObjectList, SelectedLabel.RuleList))
                {
                    var pLV = new PrintLabelView() { DataContext = pVM };
                    pWin.SetPrintLabelView(pLV);

                    pWin.Owner = Application.Current.MainWindow;
                    pWin.ShowDialog();
                }
                else
                    DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "경고", "2개 이상의 '시리얼 넘버' 규칙이 사용되었습니다.");

            }
        }

        public ICommand ClickShowVersion { get; private set; }
        private void ShowVersion()
        {
            string msg = " 제품 명: \t" + SystemInfo.Name;
            msg += "\n 버전 : \t\t" + SystemInfo.Version;
            msg += "\n 프로그램 제작일: \t" + SystemInfo.ReleaseDate.ToString("yyyy.MM.dd");
            msg += "\n 라이선스 키 제조일: \t" + Namkang.License.Controller.ProgramLicense.ManufacturedDate.ToString("yyyy.MM.dd");
            msg += "\n 서비스 종료일: \t\t" + Namkang.License.Controller.ProgramLicense.ServiceExpirationDate.ToString("yyyy.MM.dd");
            msg += "\n\n Developed by NAMKANG HI-TECH CO., LTD.";
            DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, Language.MenuHelpProgInfo, msg);
        }

        private bool HasLicense()
        {
            string hardLockLoginErrMsg = Namkang.License.Controller.Login();

            if (string.IsNullOrEmpty(hardLockLoginErrMsg))
                return true;
            else
            {
                DialogService.ShowSimpleTextDialog("경고", hardLockLoginErrMsg);
                return false;
            }
        }

    }
}
