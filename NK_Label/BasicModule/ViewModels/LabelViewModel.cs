using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Rule;
using BasicModule.ViewModels.Option;
using BasicModule.Views.Option;
using BasicModule.Utils;
using System.Windows;

namespace BasicModule.ViewModels
{
    public class LabelViewModel : BindableBase
    {
        #region Properties

        public readonly string LabelVersion = "1.0.0";

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

        private BasicObject _selectedObject;
        public BasicObject SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                SetProperty(ref _selectedObject, value);
                ChangeOptionRegion();
            }
        }

        private List<RuleMain> _ruleList;
        public List<RuleMain> RuleList
        {
            get { return _ruleList; }
            set { SetProperty(ref _ruleList, value); }
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
            newView.DataContext = new OptionLabelViewModel(_label);
            _regionManager.Regions["OptionRegion"].Add(newView, null, true);

            SelectedCommand = new DelegateCommand<object[]>(OnItemSelected);
            ClickReleaseAll = new DelegateCommand(ReleaseObject);
            ClickDeleteObject = new DelegateCommand(DeleteObject);
            
            //TestSource();
        }
        #endregion //Constructor

        private void TestSource()
        {
            RuleMain rt = new RuleMain()
            {
                Format = RuleRregulation.RuleFormat.TIME,
                Name = "Time",
                Description = "Test Rule Time"
            };
            var rtt = new RuleTime()
            {
                Pattern = "MMM dd(hh:mm tt)"
            };
            rt.Content = rtt;
            RuleList.Add(rt);

            RuleMain rl = new RuleMain()
            {
                Format = RuleRregulation.RuleFormat.MANUAL_LIST,
                Name = "List",
                Description = "Test Rule List"
            };
            RuleManualList rml = new RuleManualList()
            {
                ContentList = new Dictionary<string, string>()
            };
            rml.AddList("key1", "Description1");
            rml.AddList("Key2", "Description2");
            rml.AddList("Key3", "Description3");
            rml.SelectedContent = "Key2";
            rl.Content = rml;
            RuleList.Add(rl);

            RuleMain rs = new RuleMain()
            {
                Format = RuleRregulation.RuleFormat.SEQUENTIAL_NUM,
                Name = "Serial",
                Description = "Test Rule Serial"
            };
            var rsn = new RuleSequentialNum()
            {
                NumLength = 5,
                MaxNum = 10,
                MinNum = 1,
                Increment = 1
            };
            rs.Content = rsn;
            RuleList.Add(rs);

        }

        #region Common Event

        public ICommand ClickReleaseAll { get; private set; }
        private void ReleaseObject()
        {
            if (SelectedObject != null)
            {
                if (SelectedObject is BarcodeObject)
                    (SelectedObject as BarcodeObject).IsSelected = false;
                else if (SelectedObject is TextObject)
                    (SelectedObject as TextObject).IsSelected = false;
            }
            SelectedObject = null;
        }

        public ICommand ClickDeleteObject { get; private set; }
        public void DeleteObject()
        {
            if (SelectedObject != null)
            {
                var msg = "";
                if(SelectedObject is TextObject)
                {
                    var to = SelectedObject as TextObject;
                    msg = "Name : " + to.Name
                        + "\nText : " + to.Text 
                        + "\n\n을(를) 삭제하시겠습니까?";
                }else if(SelectedObject is BarcodeObject)
                {
                    var bo = SelectedObject as BarcodeObject;
                    msg = "Name : " + bo.Name
                        + "\nType : " + bo.BarcodeType
                        + "\nText : " + bo.Text
                        + "\n\n을(를) 삭제하시겠습니까?";
                }

                if (new DialogService().ShowSimpleSelectDialog(Application.Current.MainWindow, "Alarm", msg) == true)
                {
                    ObjectList.Remove(SelectedObject);
                    SelectedObject = null;
                }
            }
        }

        public DelegateCommand<object[]> SelectedCommand { get; private set; }
        public void OnItemSelected(object[] selectedItems)
        {
            if (selectedItems != null && selectedItems.Count() > 0)
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

        #endregion Common Events

        #region Functions
        
        public bool IsChanged
        {
            get
            {
                if (Label.Changed)
                    return true;

                foreach (var obj in ObjectList)
                    if (obj.Changed)
                        return true;

                return false;
            }
            set
            {
                Label.Changed = value;
                foreach (var obj in ObjectList)
                    obj.Changed = value;
            }
        }

        public List<RuleMain> CloneRuleList
        {
            get{

                var newList = new List<RuleMain>();
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
