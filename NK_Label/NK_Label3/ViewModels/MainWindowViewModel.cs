using BasicModule.Files;
using BasicModule.ViewModels;
using BasicModule.Views;
using NK_Label3.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BasicModule.Models;
using BasicModule.Utils;
using System.Windows;

namespace NK_Label3.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        #region System Properties

        private string _title = "NK-Label 1.0";
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

        #region Event Properties
        private IDialogService DialogService { get { return new DialogService(); } }

        private ObservableCollection<LabelView> _labelViewList = new ObservableCollection<LabelView>();
        public ObservableCollection<LabelView> LabelViewList
        {
            get { return _labelViewList; }
            set { SetProperty(ref _labelViewList, value); }
        }
        private LabelView _selectedLabelView;
        public LabelView SelectedLabelView
        {
            get { return _selectedLabelView; }
            set { SetProperty(ref _selectedLabelView, value); }
        }

        #endregion //Event Properties
        
        private readonly IRegionManager _regionManager;
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            _language = new SystemLanguage();
            //language.Load();

            ClickAddNewLabel = new DelegateCommand(AddNewLabel);
            ClickCloseCurrentLabel = new DelegateCommand(CloseCurrentLabel);
            ClickCloseAllLabel = new DelegateCommand(CloseAllLabel);

            ClickOpen = new DelegateCommand(Open);
            ClickSave = new DelegateCommand(Save);
            ClickSaveAs = new DelegateCommand(SaveAs);
        }

        #region Label Control Events

        private void AddLabel(LabelViewModel newLVM)
        {
            if (newLVM != null)
            {
                var newView = new LabelView();
                newView.DataContext = newLVM;
                _labelViewList.Add(newView);
                SelectedLabelView = newView;
                
                _regionManager.Regions["ContentRegion"].Add(newView, null, true);
                _regionManager.Regions["ContentRegion"].Activate(newView);
            }
        }

        public ICommand ClickAddNewLabel { get; private set; }
        private void AddNewLabel()
        {
            var newLabel = new LabelObject();
            var olViewModel = new OptionLabelViewModel(newLabel); ;
            var olView = new OptionLabelView();
            olView.DataContext = olViewModel;
            
            var dialogResult = DialogService.ShowDialog(Application.Current.MainWindow, olView, "Create New Label");

            if (dialogResult == true)
            {
                var newLVM = new LabelViewModel(_regionManager);
                newLVM.Label = newLabel;
                AddLabel(newLVM);
            }
        }

        public ICommand ClickCloseCurrentLabel { get; private set; }
        private void CloseCurrentLabel()
        {
            if (LabelViewList.Count > 0 && SelectedLabelView != null)
            {
                _regionManager.Regions["ContentRegion"].Remove(SelectedLabelView);
                LabelViewList.Remove(SelectedLabelView);
                SelectedLabelView = LabelViewList[0];
            }
        }
        public ICommand ClickCloseAllLabel { get; private set; }
        private void CloseAllLabel()
        {
            if (LabelViewList.Count > 0 && SelectedLabelView != null)
            {
                _regionManager.Regions["ContentRegion"].RemoveAll();
                LabelViewList.Clear();
                SelectedLabelView = null;
            }
        }
        #endregion //Label Control Events

        #region File Control Events

        public ICommand ClickOpen { get; private set; }
        private void Open()
        {
            AddLabel(FileController.OpenLabel(_regionManager));
        }

        public ICommand ClickSave { get; private set; }
        private void Save()
        {
            if (SelectedLabelView != null)
            {
                var labelVM = SelectedLabelView.DataContext as LabelViewModel;
                FileController.SaveLabel(ref labelVM, false);
            }
        }
        public ICommand ClickSaveAs { get; private set; }
        private void SaveAs()
        {
            if (SelectedLabelView != null)
            {
                var labelVM = SelectedLabelView.DataContext as LabelViewModel;
                FileController.SaveLabel(ref labelVM, true);
            }
        }

        #endregion //File Control Events
    }
}
