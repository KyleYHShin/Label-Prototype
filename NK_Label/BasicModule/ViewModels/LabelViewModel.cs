using BasicModule.Common;
using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Rule;
using BasicModule.Utils;
using BasicModule.ViewModels.Option;
using BasicModule.Views.Option;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.ViewModels
{
    public class LabelViewModel : NotifyPropertyChanged
    {
        #region Properties

        public readonly int FileVersion = 1;

        private string _filePath;
        public string FilePath { get { return _filePath; } set { _filePath = value; OnPropertyChanged(); } }

        private LabelObject _label = new LabelObject();
        public LabelObject Label
        {
            get { return _label; }
            set { _label = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BasicObject> _objectList;
        public ObservableCollection<BasicObject> ObjectList
        {
            get { return _objectList; }
            set { _objectList = value; OnPropertyChanged(); }
        }

        private BasicObject _selectedObject;
        public BasicObject SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value; OnPropertyChanged();
                ChangeOptionRegion();
            }
        }

        private List<RuleMain> _ruleList;
        public List<RuleMain> RuleList
        {
            get { return _ruleList; }
            set { _ruleList = value; OnPropertyChanged(); }
        }

        #endregion Properties

        #region Constructor

        private readonly IRegionManager _regionManager;
        public LabelViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            ObjectList = new ObservableCollection<BasicObject>();
            RuleList = new List<RuleMain>();

            var newView = new OptionLabelView();
            newView.DataContext = new OptionLabelViewModel(Label, false);
            _regionManager.Regions["OptionRegion"].Add(newView, null, true);

            SelectedCommand = new DelegateCommand<object[]>(OnItemSelected);
            ClickReleaseAll = new DelegateCommand(ReleaseObject);
            ClickDeleteObject = new DelegateCommand(DeleteObject);
        }

        #endregion Constructor

        #region Common Event

        public ICommand ClickReleaseAll { get; private set; }
        private void ReleaseObject()
        {
            if (SelectedObject != null)
                SelectedObject.IsSelected = false;

            SelectedObject = null;
        }

        public ICommand ClickDeleteObject { get; private set; }
        public void DeleteObject()
        {
            if (SelectedObject != null)
            {
                var msg = "";
                if (SelectedObject is TextObject)
                {
                    var to = SelectedObject as TextObject;
                    msg = "Name : " + to.Name
                        + "\nText : " + to.Text
                        + "\n\n을(를) 삭제하시겠습니까?";
                }
                else if (SelectedObject is BarcodeObject)
                {
                    var bo = SelectedObject as BarcodeObject;
                    msg = "Name : " + bo.Name
                        + "\nType : " + bo.BarcodeType
                        + "\nText : " + bo.Text
                        + "\n\n을(를) 삭제하시겠습니까?";
                }

                if (DialogService.ShowSimpleSelectDialog(Application.Current.MainWindow, "알림", msg) == true)
                {
                    ObjectList.Remove(SelectedObject);
                    SelectedObject = null;
                }
            }
        }

        public DelegateCommand<object[]> SelectedCommand { get; private set; }
        public void OnItemSelected(object[] selectedItems)
        {
            if (selectedItems != null && selectedItems.Length > 0)
            {
                SelectedObject = selectedItems[0] as BasicObject;
            }
        }

        public void ChangeOptionRegion()
        {
            _regionManager.Regions["OptionRegion"].RemoveAll();

            if (SelectedObject is TextObject)
            {
                var newView = new OptionTextView();
                newView.DataContext = new OptionTextViewModel(SelectedObject as TextObject, false);
                _regionManager.Regions["OptionRegion"].Add(newView, null, true);
            }
            else if (SelectedObject is BarcodeObject)
            {
                var newView = new OptionBarcodeView();
                newView.DataContext = new OptionBarcodeViewModel(SelectedObject as BarcodeObject, false);
                _regionManager.Regions["OptionRegion"].Add(newView, null, true);
            }
            else
            {
                var newView = new OptionLabelView();
                newView.DataContext = new OptionLabelViewModel(Label, false);
                _regionManager.Regions["OptionRegion"].Add(newView, null, true);
            }
        }

        #endregion Common Events

        #region Functions

        public bool IsChanged
        {
            get
            {
                if (Label.IsChanged)
                    return true;

                foreach (var obj in ObjectList)
                    if (obj.IsChanged)
                        return true;

                foreach (var rule in RuleList)
                    if (rule.IsChanged)
                        return true;

                return false;
            }
            set
            {
                Label.IsChanged = value;
                foreach (var obj in ObjectList)
                    obj.IsChanged = value;

                foreach (var rule in RuleList)
                    rule.IsChanged = value;
            }
        }

        public ObservableCollection<RuleMain> CloneObservableRuleList
        {
            get
            {
                var newList = new ObservableCollection<RuleMain>();
                foreach (RuleMain r in RuleList)
                {
                    newList.Add(r.Clone as RuleMain);
                }
                return newList;
            }
        }

        #endregion Functions
    }
}
