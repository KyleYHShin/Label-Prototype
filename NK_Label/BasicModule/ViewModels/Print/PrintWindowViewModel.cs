using BasicModule.Common;
using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Option;
using BasicModule.Models.Rule;
using BasicModule.Utils;
using BasicModule.Views.Print;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.ViewModels.Print
{
    public class PrintWindowViewModel : BindableBase
    {
        private readonly string PRINT_VIEW_NAME = "pView";
        #region Label Properties

        private LabelObject _label;
        public LabelObject Label { get { return _label; } set { SetProperty(ref _label, value); } }

        private ObservableCollection<BasicObject> _objectList;
        public ObservableCollection<BasicObject> ObjectList { get { return _objectList; } set { SetProperty(ref _objectList, value); } }

        private List<RuleMain> _ruleList;
        public List<RuleMain> RuleList { get { return _ruleList; } set { SetProperty(ref _ruleList, value); } }

        private ObservableCollection<RuleMain> _seqRuleList;
        public ObservableCollection<RuleMain> SeqRuleList { get { return _seqRuleList; } set { SetProperty(ref _seqRuleList, value); } }

        private ObservableCollection<RuleMain> _manuRuleList;
        public ObservableCollection<RuleMain> ManuRuleList { get { return _manuRuleList; } set { SetProperty(ref _manuRuleList, value); } }

        private ObservableCollection<RuleMain> _timeRuleList;
        public ObservableCollection<RuleMain> TimeRuleList { get { return _timeRuleList; } set { SetProperty(ref _timeRuleList, value); } }

        #endregion Label Properties

        #region Printer Properties

        private PrintService pService;

        public object PrinterList { get => PrinterOption.PrinterList; }
        public object DpiList { get => PrinterOption.DpiList; }

        private bool _repetable;
        public bool Repetable { get { return _repetable; } set { SetProperty(ref _repetable, value); } }

        private int _repetition = 1;
        public int Repetition
        {
            get { return _repetition; }
            set
            {
                if (value > 0)
                    SetProperty(ref _repetition, value);
            }
        }

        public List<string> _usablePrinterList;
        public List<string> UsablePrinterList { get { return _usablePrinterList; } set { SetProperty(ref _usablePrinterList, value); } }

        private string _selectedPrinterName;
        public string SelectedPrinterName { get { return _selectedPrinterName; } set { SetProperty(ref _selectedPrinterName, value); } }

        private int _offsetX;
        public int OffsetX
        {
            get { return _offsetX; }
            set
            {
                if (value >= 0)
                    SetProperty(ref _offsetX, value);
            }
        }
        private int _offsetY;
        public int OffsetY
        {
            get { return _offsetY; }
            set
            {
                if (value >= 0)
                    SetProperty(ref _offsetY, value);
            }
        }

        #endregion Printer Properties

        #region Constructor

        private readonly IRegionManager RegionManager;
        public PrintWindowViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;

            Refresh = new DelegateCommand(RefreshView);
        }

        #endregion Constructor
        #region Functions

        public void Initialize(LabelObject label, ObservableCollection<BasicObject> originalObjectList, List<RuleMain> originalRuleList)
        {
            Label = label; //swallow copy

            ObjectList = new ObservableCollection<BasicObject>(); //deep copy
            foreach (var obj in originalObjectList)
            {
                if (obj is IPrintableObject)
                {
                    IPrintableObject newObj = (obj as IPrintableObject).Clone;
                    newObj.OriginText = newObj.Text;
                    newObj.Text = "";
                    ObjectList.Add(newObj as BasicObject);
                }
            }

            InitializeRuleData(originalRuleList);

            pService = new PrintService();
            UsablePrinterList = pService.GetUsablePrinterList();
            if (UsablePrinterList.Count > 0)
                SelectedPrinterName = UsablePrinterList[0];

            if (SeqRuleList.Count > 0)
                Repetable = false;
            else
                Repetable = true;

            RefreshLabelView();
        }

        private void InitializeRuleData(List<RuleMain> originalRuleList)
        {
            // swallow copy only used Rule
            RuleList = new List<RuleMain>();
            foreach (var obj in ObjectList)
            {
                if (!(obj is IPrintableObject))
                    continue;

                var text = (obj as IPrintableObject).OriginText;
                if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                    continue;
                var wordList = RuleRegulation.RuleNameSeperatorToList(text);
                foreach (var word in wordList)
                {
                    if (!RuleRegulation.RuleNameVerifier(word))
                        continue;

                    var rName = RuleRegulation.RuleNameExtractor(word);
                    foreach (var r in originalRuleList)
                    {
                        if (r.Name.Equals(rName) && !RuleList.Contains(r))
                        {
                            RuleList.Add(r);
                            break;
                        }
                    }
                }
            }

            SeqRuleList = new ObservableCollection<RuleMain>();
            ManuRuleList = new ObservableCollection<RuleMain>();
            TimeRuleList = new ObservableCollection<RuleMain>();
            foreach (var r in RuleList)
            {
                if (r.Content is RuleSequentialNum)
                    SeqRuleList.Add(r);
                else if (r.Content is RuleManualList)
                    ManuRuleList.Add(r);
                else if (r.Content is RuleTime)
                    TimeRuleList.Add(r);
            }

            RegionManager.Regions[RegionNames.PrintRuleSeq].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleManu].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleTime].RemoveAll();

            var seqListView = new PrintRuleSequentialNumView();
            seqListView.DataContext = this;
            RegionManager.Regions[RegionNames.PrintRuleSeq].Add(seqListView, null, true);

            var manuListView = new PrintRuleManualListView();
            manuListView.DataContext = this;
            RegionManager.Regions[RegionNames.PrintRuleManu].Add(manuListView, null, true);

            var timeListView = new PrintRuleTimeView();
            timeListView.DataContext = this;
            RegionManager.Regions[RegionNames.PrintRuleTime].Add(timeListView, null, true);
        }

        private List<RuleMain> RuleListInText(List<RuleMain> originalRuleList, string text)
        {
            var ret = new List<RuleMain>();

            var wordList = RuleRegulation.RuleNameSeperatorToList(text);
            foreach (var word in wordList)
            {
                if (!RuleRegulation.RuleNameVerifier(word))
                    continue;

                var rName = RuleRegulation.RuleNameExtractor(word);
                foreach (var r in originalRuleList)
                {
                    if (r.Name.Equals(rName) && !ret.Contains(r) && !RuleList.Contains(r))
                    {
                        ret.Add(r);
                        break;
                    }
                }
            }

            return ret;
        }

        private void ConvertRuleToText()
        {
            foreach (var obj in ObjectList)
            {
                if (!(obj is IPrintableObject))
                    continue;

                var pObj = obj as IPrintableObject;
                if (string.IsNullOrEmpty(pObj.OriginText))
                    continue;

                var wordList = RuleRegulation.RuleNameSeperatorToList(pObj.OriginText);
                for (int i = 0; i < wordList.Count; i++)
                {
                    if (RuleRegulation.RuleNameVerifier(wordList[i]))
                    {
                        var rName = RuleRegulation.RuleNameExtractor(wordList[i]);
                        foreach (RuleMain r in RuleList)
                        {
                            if (r.Name.Equals(rName))
                            {
                                wordList[i] = r.PrintValue;
                                break;
                            }
                        }
                    }
                }

                pObj.Text = string.Join("", wordList.ToArray());
                if (pObj.Text.Length > pObj.MaxLength)
                    pObj.Text = pObj.Text.Substring(0, pObj.MaxLength);
            }
        }
        
        public ICommand Refresh { get; private set; }
        private void RefreshView()
        {
            RefreshLabelView();
        }
        private PrintLabelView RefreshLabelView()
        {
            RegionManager.Regions[RegionNames.PrintLabelView].RemoveAll();

            ConvertRuleToText();

            var pLabelView = new PrintLabelView();
            pLabelView.DataContext = this;
            RegionManager.Regions[RegionNames.PrintLabelView].Add(pLabelView, PRINT_VIEW_NAME);

            return pLabelView;
        }

        #endregion Functions

        #region Print
        private void EnableAllControls(bool canUse)
        {
            //
        }

        public void StartPrint(PrintLabelView PLView)
        {
            EnableAllControls(false);
            try
            {
                switch (SeqRuleList.Count)
                {
                    case 0:
                        for (var i = 0; i < Repetition; i++)
                        {
                            //ConvertRuleToText();
                            RefreshLabelView();
                            Print(PLView);
                        }
                        break;
                    case 1:
                        RuleSequentialNum rsn = SeqRuleList[0].Content as RuleSequentialNum;
                        for (var i = rsn.CurrNum; i <= rsn.MaxNum; i += rsn.Increment)
                        {
                            rsn.CurrNum = i;
                            //ConvertRuleToText();
                            var asfdaview = RefreshLabelView();
                            Print(asfdaview);
                            //var view = RegionManager.Regions[RegionNames.PrintLabelView].GetView(PRINT_VIEW_NAME);
                            //if (view is PrintLabelView)
                            //{
                            //    PrintLabelView nView = view as PrintLabelView;
                            //    Print(view as PrintLabelView);
                            //}

                            //System.Threading.Thread.Sleep(500);
                            //Print(PLView);
                        }
                        break;
                    default:
                        DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "Exception", "There are a lot of Serial Rules.");
                        break;
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "Exception", ex.Source + ex.Message + "\n" + ex.StackTrace);
            }
            EnableAllControls(true);
        }

        private void Print(PrintLabelView PLView)
        {
            switch (Label.SelectedPrinter)
            {
                case PrinterOption.PrinterType.ZEBRA:
                    var zplCode = new System.Text.StringBuilder();
                    zplCode.AppendFormat("^XA");
                    zplCode.AppendFormat("^FO{0},{1}", OffsetX, OffsetY);
                    zplCode.AppendFormat("{0}", BitmapConversion.ConvertImageToZPLString(PLView));
                    zplCode.AppendFormat("^FS");
                    zplCode.AppendFormat("^XZ");

                    Console.WriteLine(zplCode);
                    //pService.PrintZebraProduct(SelectedPrinterName, zplCode.ToString());
                    break;
            }
        }

        #endregion Print
    }
}
