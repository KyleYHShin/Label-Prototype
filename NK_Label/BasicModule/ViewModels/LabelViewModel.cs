using System;
using System.Collections.ObjectModel;
using System.Linq;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using BasicModule.Models;
using BasicModule.ViewModels.Option;
using BasicModule.Views.Option;

namespace BasicModule.ViewModels
{
    public class LabelViewModel : BindableBase
    {
        #region Properties

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        private LabelObject _label = new LabelObject();
        public LabelObject Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        private ObservableCollection<BasicObject> _objectList;
        public ObservableCollection<BasicObject> ObjectList
        {
            get { return _objectList; }
            set { SetProperty(ref _objectList, value); }
        }

        #endregion //Properties
        
        #region Constructor

        private readonly IRegionManager _regionManager;
        public LabelViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            ObjectList = new ObservableCollection<BasicObject>();

            var newView = new OptionLabelView();
            newView.DataContext = new OptionLabelViewModel(_label);
            _regionManager.Regions["OptionRegion"].Add(newView, null, true);

            SelectedCommand = new DelegateCommand<object[]>(OnItemSelected);
        }

        #endregion //Constructor

        #region Common Event

        public DelegateCommand<object[]> SelectedCommand { get; private set; }
        private Object _selectedObject;
        public Object SelectedObject
        {
            get { return _selectedObject; }
            private set { SetProperty(ref _selectedObject, value); }
        }

        public void OnItemSelected(object[] selectedItems)
        {
            if (selectedItems != null && selectedItems.Count() > 0)
            {
                SelectedObject = selectedItems[0];
                ChangeOptionRegion();
            }
        }

        public void ChangeOptionRegion()
        {
            _regionManager.Regions["OptionRegion"].RemoveAll();

            if (SelectedObject is TextObject)
            {
                var newView = new OptionTextView();
                newView.DataContext = new OptionTextViewModel(SelectedObject as TextObject);
                _regionManager.Regions["OptionRegion"].Add(newView, null, true);
            }
            else if (SelectedObject is BarcodeObject)
            {
                var newView = new OptionBarcodeView();
                newView.DataContext = new OptionBarcodeViewModel(SelectedObject as BarcodeObject);
                _regionManager.Regions["OptionRegion"].Add(newView, null, true);
            }
            else
            {
                var newView = new OptionLabelView();
                newView.DataContext = new OptionLabelViewModel(_label);
                _regionManager.Regions["OptionRegion"].Add(newView, null, true);
            }
        }

        #endregion //Common Events

        #region Functions

        public bool IsChanged
        {
            get
            {
                var ret = Label.Changed;
                foreach (var obj in ObjectList)
                {
                    ret ^= obj.Changed;
                }
                return ret;
            }
            set
            {
                Label.Changed = value;
                foreach (var obj in ObjectList)
                {
                    obj.Changed = value;
                }
            }
        }

        #endregion //Functions

        #region Rule
        //룰 프로퍼티
        //룰

        #endregion

    }
}
