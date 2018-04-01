using System;
using System.Collections.ObjectModel;
using System.Linq;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using BasicModule.Models;
using BasicModule.Models.Rule;
using BasicModule.ViewModels.Option;
using BasicModule.Views.Option;
using System.Collections.Generic;

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

        private List<Rule> _ruleList;
        public List<Rule> RuleList
        {
            get { return _ruleList; }
            set { SetProperty(ref _ruleList, value); }
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

        private void TestSource()
        {
            while (true)
            {
                Rule r = new Rule()
                {
                    Format = RuleRregulation.RuleFormat.MANUAL_LIST,
                    Name = "Rule1",
                    Description = "Test Rule"
                };

                //List 출력
                //RuleManualList rml = new RuleManualList()
                //{
                //    ContentList = new Dictionary<string, string>()
                //};
                //r.Content = rml;

                //bool ret = rml.AddList("key1", "value1");
                //ret = rml.AddList("Key1", "value2");
                //ret = rml.AddList("fda@@", "dfdf");
                //rml.SelectedContent = "value2";

                //Sequential 출력
                //var rsn = new RuleSequentialNum()
                //{
                //    NumLength = 5,
                //    MaxNum = 10,
                //    MinNum = 1,
                //    Increment = 1
                //};
                //r.Content = rsn;
                //for (var i = rsn.CurrNum; i <= rsn.MaxNum; i += rsn.Increment)
                //{
                //    Console.WriteLine(r.PrintValue());
                //}

                //Time 출력
                //var rt = new RuleTime()
                //{
                //    Pattern = "yyyy-MM-dd : HH:mm:ss"
                //};
                //r.Content = rt;
                //Console.WriteLine(r.PrintValue());

            }
        }
        
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

        public bool RuleNameDuplicationVerifier(Rule rule)
        {
            foreach (var r in RuleList)
                if (r.Name.Equals(rule.Name))
                    return false;

            return true;
        }

        #endregion //Functions
    }
}
