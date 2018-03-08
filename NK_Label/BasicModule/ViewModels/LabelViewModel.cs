using BasicModule.Models;
using BasicModule.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

        private ObservableCollection<BasicObject> _objectList = new ObservableCollection<BasicObject>();
        public ObservableCollection<BasicObject> ObjectList
        {
            get { return _objectList; }
            set { SetProperty(ref _objectList, value); }
        }

        //public OptionViewModel OptionViewModel { get; set; }

        #endregion //Properties

        #region Event Properties

        public DelegateCommand<object[]> SelectedCommand { get; private set; }
        private Object _selectedObject;
        public Object SelectedObject
        {
            get { return _selectedObject; }
            private set { SetProperty(ref _selectedObject, value); }
        }

        #endregion //Event Properties

        #region Constructor

        private readonly IRegionManager _regionManager;
        public LabelViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            var newView = new OptionLabelView();
            newView.DataContext = new OptionLabelViewModel(_label);
            _regionManager.Regions["OptionRegion"].Add(newView, null, true);

            SelectedCommand = new DelegateCommand<object[]>(OnItemSelected);
        }

        #endregion //Constructor

        #region Events

        private void OnItemSelected(object[] selectedItems)
        {
            if (selectedItems != null && selectedItems.Count() > 0)
            {
                SelectedObject = selectedItems.FirstOrDefault();
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
        }

        #endregion //Events

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
                Label.Changed = false;
                foreach (var obj in ObjectList)
                {
                    obj.Changed = false;
                }
            }
        }

        #endregion //Functions

        #region Test Sources
        public void Init()
        {
            Label = new LabelObject()
            {
                Name = "First Label",
                Width = 400,
                Height = 160,
                Changed = false
            };

            var testObjList = new ObservableCollection<BasicObject>();

            testObjList.Add(new BarcodeObject()
            {
                Name = "barcode1",
                Text = "NK-Label Test1",
                PosX = 20,
                PosY = 40,
                Width = 80,
                Height = 80,
                BarcodeType = "DATA_MATRIX",
                Changed = false
            });
            testObjList.Add(new TextObject()
            {
                Name = "tb1",
                Text = "NK-Label",
                PosX = 20,
                PosY = 120,
                Width = 70,
                Height = 20,
                Margin = new System.Windows.Thickness(50, 0, 0, 0),
                FontSize = 10,
                Changed = false
            });
            testObjList.Add(new BarcodeObject()
            {
                Name = "barcode2",
                Text = "NK-Label Test2",
                PosX = 130,
                PosY = 40,
                Width = 210,
                Height = 70,
                BarcodeType = "CODE_128",
                Changed = false
            });
            testObjList.Add(new TextObject()
            {
                Name = "tb2",
                Text = "NK-Label Test2",
                PosX = 150,
                PosY = 120,
                Width = 70,
                Height = 20,
                Margin = new System.Windows.Thickness(0, 10, 0, 0),
                FontSize = 16,
                Changed = false
            });
            ObjectList = testObjList;
        }
        #endregion //Test Sources
    }
}
