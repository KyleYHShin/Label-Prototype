using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

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

using NK_Label3.Models;

namespace NK_Label3.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        #region System Properties

        private string _title = "NK-Label 1.0.0 Beta 1";
        public string Title { get { return _title; } set { SetProperty(ref _title, value); } }

        private double _width = 1280;
        public double Width { get { return _width; } set { SetProperty(ref _width, value); } }

        private double _height = 720;
        public double Height { get { return _height; } set { SetProperty(ref _height, value); } }

        private string _background = "#FFDEDEDE";
        public string Background { get { return _background; } set { SetProperty(ref _background, value); } }

        private SystemLanguage _language;
        public SystemLanguage Language { get { return _language; } set { SetProperty(ref _language, value); } }

        #endregion System Properties

        #region Tab Contents

        private ObservableCollection<LabelView> _labelViewList;
        public ObservableCollection<LabelView> LabelViewList { get { return _labelViewList; } set { SetProperty(ref _labelViewList, value); } }

        private LabelView _selectedLabelView;
        public LabelView SelectedLabelView
        {
            get { return _selectedLabelView; }
            set
            {
                SetProperty(ref _selectedLabelView, value);
                if (value is LabelView)
                {
                    (value.DataContext as LabelViewModel).ChangeOptionRegion();
                    HasLabel = true;
                }
                else
                {
                    HasLabel = false;
                }
            }
        }

        private bool _hasLabel;
        public bool HasLabel { get { return _hasLabel; } set { SetProperty(ref _hasLabel, value); } }

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

        //private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    var ret =CanCloseAllLabel();
        //    if (ret)
        //        e.Cancel = false;
        //    else
        //        e.Cancel = true;
        //    //if ((DataContext as MainWindowViewModel).CanCloseAllLabel())
        //    //{
        //    //    e.Cancel = true;
        //    //}
        //}

        public ICommand ClickCloseWindow { get; private set; }
        private void CloseWindow()
        {
            if(CanCloseAllLabel())
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
                var newLVM = new LabelViewModel(RegionManager);
                newLVM.Label = newLabel;
                AddLabel(newLVM);
            }
        }

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

        #endregion Label Object Events

        #region Rule Events

        public ICommand ClickEidtRuleList { get; private set; }
        private void EditRuleList()
        {
            if(SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var LVM = SelectedLabelView.DataContext as LabelViewModel;
                var ruleEditView = new RuleListView(RegionManager);
                (ruleEditView.DataContext as RuleListViewModel).RuleList = LVM.CloneObservableRuleList;

                if (DialogService.ShowSelectDialog(Application.Current.MainWindow, ruleEditView, "Edit Rules") == true)
                {
                    LVM.RuleList.Clear();
                    foreach(var cr in (ruleEditView.DataContext as RuleListViewModel).RuleList)
                        LVM.RuleList.Add(cr);
                }
            }
        }

        #endregion Rule Events

        #region File Control Events

        public ICommand ClickOpen { get; private set; }
        private void Open()
        {
            LabelViewModel newLVM = FileController.OpenLabel(RegionManager);
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

        #endregion File Control Events
                
        public ICommand ClickPrintCurrentLabel { get; private set; }
        private void PrintCurrentLabel()
        {
            if (SelectedLabelView != null && SelectedLabelView.DataContext is LabelViewModel)
            {
                var pWin = new PrintWindow(RegionManager) { Title = "Print Label" };

                var LVM = SelectedLabelView.DataContext as LabelViewModel;
                var pVM = pWin.DataContext as PrintWindowViewModel;
                if(pVM.Initialize(LVM.Label, LVM.ObjectList, LVM.RuleList))
                {
                    var pLV = new PrintLabelView() { DataContext = pVM };
                    pWin.SetPrintLabelView(pLV);

                    pWin.Owner = Application.Current.MainWindow;
                    pWin.ShowDialog();
                }
                else
                    DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "Warning", "너무 많은 Serial 규칙을 사용하고 있습니다.");

            }
        }


        public ICommand ClickShowVersion { get; private set; }
        private void ShowVersion()
        {
            string msg = "Product Name: \tNK-Label";
            msg += "\nVersion : \t1.0.0 Beta 1";
            msg += "\nRelease : \t2018.05.03";
            msg += "\n\nDeveloped by NAMKANG HI-TECH CO., LTD.";
            DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "Version Information", msg);
        }
    }
}
