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
                newView.ViewModel = newLVM;
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
            var cVM = new ChildWindowViewModel();
            cVM.CurrentViewModel = new OptionLabelViewModel(newLabel);

            var cWin = new ChildWindow();
            cWin.Owner = Application.Current.MainWindow;
            cWin.Title = "Create New Label";
            cWin.DataContext = cVM;
            try
            {
                Application.Current.MainWindow.IsEnabled = false;
                cWin.Show();
                //if dialogResult==ok -> Create new LabelView by newLabel -> Active LabelView
                //1. How to get signal from ChildWindow
                //2. How to get LabelObject from ChildWindow
                //  -> check newLabel object
                //LabelViewModel newLVM = new LabelViewModel(_regionManager);
                //newLVM.Label = newLabel;
                //AddLabel(newLVM);



                //Origin Test Source
                //var newViewModel = new LabelViewModel(_regionManager);
                //newViewModel.Init();
                //var newView = new LabelView();
                //newView.DataContext = newViewModel;
                //newView.ViewModel = newViewModel;
                //_labelViewList.Add(newView);
                //SelectedLabelView = newView;

                //_regionManager.Regions["ContentRegion"].Add(newView, null, true);
                //_regionManager.Regions["ContentRegion"].Activate(newView);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Application.Current.MainWindow.IsEnabled = true;
                cWin.Close();
            }
        }

        public ICommand ClickCloseCurrentLabel { get; private set; }
        private void CloseCurrentLabel()
        {
            if (LabelViewList.Count > 0 && SelectedLabelView != null)
            {
                //if (SelectedLabelView.ViewModel.IsChanged)
                //{
                //    Console.log(SelectedLabelView.ViewModel.Label.Name + "is changed");
                //}
                _regionManager.Regions["ContentRegion"].Remove(SelectedLabelView);
                LabelViewList.Remove(SelectedLabelView);
                SelectedLabelView = LabelViewList[0];
                //tab 연동
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
                var labelVM = SelectedLabelView.ViewModel;
                FileController.SaveLabel(ref labelVM, false);
            }
        }
        public ICommand ClickSaveAs { get; private set; }
        private void SaveAs()
        {
            if (SelectedLabelView != null)
            {
                var labelVM = SelectedLabelView.ViewModel;
                FileController.SaveLabel(ref labelVM, true);
            }
        }
        #endregion //File Control Events
    }
}
