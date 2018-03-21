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
using System;

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

        private ObservableCollection<LabelViewModel> _labelViewModelList;
        public ObservableCollection<LabelViewModel> LabelViewModelList
        {
            get { return _labelViewModelList; }
            set { SetProperty(ref _labelViewModelList, value); }
        }

        private LabelViewModel _selectedLabelViewModel;
        public LabelViewModel SelectedLabelViewModel
        {
            get { return _selectedLabelViewModel; }
            set
            {
                SetProperty(ref _selectedLabelViewModel, value);
                if(_selectedLabelViewModel != null)
                    _selectedLabelViewModel.ChangeOptionRegion();
            }
        }

        #endregion //Event Properties

        private readonly IRegionManager _regionManager;
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            Language = new SystemLanguage();
            //language.Load();
            LabelViewModelList = new ObservableCollection<LabelViewModel>();

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
                //var newView = new LabelView();
                //newView.DataContext = newLVM;
                LabelViewModelList.Add(newLVM);
                SelectedLabelViewModel = newLVM;
            }
        }

        public ICommand ClickAddNewLabel { get; private set; }
        private void AddNewLabel()
        {
            var newLabel = new LabelObject();
            var olViewModel = new OptionLabelViewModel(newLabel); ;
            var olView = new OptionLabelView();
            olView.DataContext = olViewModel;

            if (DialogService.ShowDialog(Application.Current.MainWindow, olView, "새 라벨 만들기") == true)
            {
                var newLVM = new LabelViewModel(_regionManager);
                newLVM.Label = newLabel;
                AddLabel(newLVM);
            }
        }

        private bool CanCloseCurrentLabel()
        {
            if (SelectedLabelViewModel is LabelViewModel)
            {
                if (SelectedLabelViewModel != null && SelectedLabelViewModel.IsChanged)
                {
                    if (DialogService.ShowSimpleDialog(Application.Current.MainWindow, "경고", "'" + SelectedLabelViewModel.Label.Name + "'에 수정된 항목이 있습니다.\n 무시하고 종료하시겠습니까?") == true)
                    {
                        _regionManager.Regions["OptionRegion"].RemoveAll();
                        LabelViewModelList.Remove(SelectedLabelViewModel);
                        return true;
                    }
                }
                else
                {
                    _regionManager.Regions["OptionRegion"].RemoveAll();
                    LabelViewModelList.Remove(SelectedLabelViewModel);
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
            while (LabelViewModelList.Count > 0 && SelectedLabelViewModel != null)
            {
                if (!CanCloseCurrentLabel())
                    break;
            }
        }
        #endregion //Label Control Events

        #region File Control Events

        public ICommand ClickOpen { get; private set; }
        private void Open()
        {
            LabelViewModel newLVM = FileController.OpenLabel(_regionManager);
            var newPath = newLVM.FilePath;
            bool isExist = false;
            foreach (LabelViewModel LVM in LabelViewModelList)
            {
                if (LVM.FilePath.Equals(newPath))
                {
                    MessageBox.Show("'" + LVM.Label.Name + "' 라벨이 이미 열려있습니다.");
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
                AddLabel(newLVM);
        }

        public ICommand ClickSave { get; private set; }
        private void Save()
        {
            if (SelectedLabelViewModel != null)
            {
                FileController.SaveLabel(ref _selectedLabelViewModel, false);
                _selectedLabelViewModel.IsChanged = false;
            }
        }
        public ICommand ClickSaveAs { get; private set; }
        private void SaveAs()
        {
            if (SelectedLabelViewModel != null)
            {
                FileController.SaveLabel(ref _selectedLabelViewModel, true);
                _selectedLabelViewModel.IsChanged = false;
            }
        }

        #endregion //File Control Events
    }
}
