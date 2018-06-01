using BasicModule.Common;
using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Option;
using BasicModule.Models.Rule;
using BasicModule.Models.Rule.Content;
using BasicModule.Utils;
using BasicModule.Views.Print;
using BasicModule.Views.Print.Content;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.ViewModels.Print
{

    public class PrintWindowViewModel : NotifyPropertyChanged
    {
        public delegate bool NKLicense();
        public NKLicense HasLicense;

        #region Label Properties

        private LabelObject _label;
        public LabelObject Label { get { return _label; } set { _label = value; OnPropertyChanged(); } }

        private ObservableCollection<BasicObject> _objectList;
        public ObservableCollection<BasicObject> ObjectList { get { return _objectList; } set { _objectList = value; OnPropertyChanged(); } }

        private List<RuleMain> UsingRuleList { get; set; }

        private ObservableCollection<RuleMain> _seqRuleList;
        public ObservableCollection<RuleMain> SeqRuleList { get { return _seqRuleList; } set { _seqRuleList = value; OnPropertyChanged(); } }

        private ObservableCollection<RuleMain> _manuRuleList;
        public ObservableCollection<RuleMain> ManuRuleList { get { return _manuRuleList; } set { _manuRuleList = value; OnPropertyChanged(); } }

        private ObservableCollection<RuleMain> _timeRuleList;
        public ObservableCollection<RuleMain> TimeRuleList { get { return _timeRuleList; } set { _timeRuleList = value; OnPropertyChanged(); } }

        private ObservableCollection<RuleMain> _inputRuleList;
        public ObservableCollection<RuleMain> InputRuleList { get { return _inputRuleList; } set { _inputRuleList = value; OnPropertyChanged(); } }

        private ObservableCollection<RuleMain> _inComRuleList;
        public ObservableCollection<RuleMain> InCombRuleList { get { return _inComRuleList; } set { _inComRuleList = value; OnPropertyChanged(); } }

        #endregion Label Properties

        #region Printer Properties

        private PrintService pService;

        public object PrinterList { get => PrinterOption.PrinterTypeList; }
        public object DpiList { get => PrinterOption.DpiList; }

        public List<string> _usablePrinterList;
        public List<string> UsablePrinterList { get { return _usablePrinterList; } set { _usablePrinterList = value; OnPropertyChanged(); } }

        private string _selectedPrinterName;
        public string SelectedPrinterName { get { return _selectedPrinterName; } set { _selectedPrinterName = value; OnPropertyChanged(); } }
        
        private bool _hasInputRule = false;
        public bool HasInputRule { get { return _hasInputRule; } set { _hasInputRule = value; OnPropertyChanged(); } }

        private bool _canRepeatOfInput = false;
        public bool CanRepeatOfInput { get { return _canRepeatOfInput; } set { _canRepeatOfInput = value; OnPropertyChanged(); } }

        private bool _isAbleToAction;
        public bool IsAbleToAction { get { return _isAbleToAction; } set { _isAbleToAction = value; OnPropertyChanged(); } }

        #endregion Printer Properties

        #region Constructor

        private readonly IRegionManager RegionManager;
        public PrintWindowViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;

            CloseCommand = new DelegateCommand(CanClose);
            Refresh = new DelegateCommand(ConvertRuleToText);
        }

        public bool Initialize(LabelObject label, ObservableCollection<BasicObject> originalObjectList, List<RuleMain> originalRuleList)
        {
            Label = label; //swallow copy

            ObjectList = new ObservableCollection<BasicObject>(); //deep copy
            foreach (var obj in originalObjectList)
            {
                if (obj is IPrintableObject)
                {
                    IPrintableObject newObj = (obj as IPrintableObject).Clone;
                    newObj.OriginText = newObj.Text;
                    newObj.Text = string.Empty;
                    ObjectList.Add(newObj as BasicObject);
                }
            }

            InitializeRule(originalRuleList);
            if (SeqRuleList.Count > 1 || InCombRuleList.Count > 1)
                return false;

            pService = new PrintService();
            UsablePrinterList = pService.GetUsablePrinterList();
            if (UsablePrinterList.Count > 0)
                SelectedPrinterName = UsablePrinterList[0];

            InitializeView();
            ConvertRuleToText();
            IsAbleToAction = true;

            return true;
        }

        private void InitializeRule(List<RuleMain> originalRuleList)
        {
            // swallow copy only used Rule
            UsingRuleList = new List<RuleMain>();
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
                        if (r.Name.Equals(rName) && !UsingRuleList.Contains(r))
                        {
                            UsingRuleList.Add(r);
                            break;
                        }
                    }
                }
            }

            SeqRuleList = new ObservableCollection<RuleMain>();
            ManuRuleList = new ObservableCollection<RuleMain>();
            TimeRuleList = new ObservableCollection<RuleMain>();
            InCombRuleList = new ObservableCollection<RuleMain>();

            foreach (var r in UsingRuleList)
            {
                switch (r.Format)
                {
                    case RuleRegulation.RuleFormat.SEQUENTIAL_NUM:
                        SeqRuleList.Add(r);
                        break;
                    case RuleRegulation.RuleFormat.MANUAL_LIST:
                        ManuRuleList.Add(r);
                        break;
                    case RuleRegulation.RuleFormat.TIME:
                        TimeRuleList.Add(r);
                        break;
                    case RuleRegulation.RuleFormat.INPUT_COMBINE:
                        InCombRuleList.Add(r);
                        break;
                }
            }

            InputRuleList = new ObservableCollection<RuleMain>();
            var sortingInputRuleList = new List<RuleMain>();
            foreach (var r in originalRuleList)
            {
                if (r.Format ==  RuleRegulation.RuleFormat.INPUT
                    && (r.Content as RuleInput).UseInPrinting)
                {
                    sortingInputRuleList.Add(r);
                }
            }
            sortingInputRuleList.Sort((x, y) => (x.Content as RuleInput).Order.CompareTo((y.Content as RuleInput).Order));
            foreach (var r in sortingInputRuleList)
                InputRuleList.Add(r);
        }

        private void InitializeView()
        {
            RegionManager.Regions[RegionNames.PrintRuleSeq].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleManu].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleTime].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleInput].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleInputCombine].RemoveAll();

            if (SeqRuleList.Count > 0)
            {
                var seqListView = new PrintRuleSequentialNumView();
                seqListView.DataContext = this;
                RegionManager.Regions[RegionNames.PrintRuleSeq].Add(seqListView, null, true);
            }

            if (ManuRuleList.Count > 0)
            {
                var manuListView = new PrintRuleManualListView();
                manuListView.DataContext = this;
                RegionManager.Regions[RegionNames.PrintRuleManu].Add(manuListView, null, true);
            }

            if (TimeRuleList.Count > 0)
            {
                var timeListView = new PrintRuleTimeView();
                timeListView.DataContext = this;
                RegionManager.Regions[RegionNames.PrintRuleTime].Add(timeListView, null, true);
            }

            if (InputRuleList.Count > 0)
            {
                HasInputRule = true;
                var inputListView = new PrintRuleInputView();
                inputListView.DataContext = this;
                RegionManager.Regions[RegionNames.PrintRuleInput].Add(inputListView, null, true);
            }

            if (InCombRuleList.Count > 0)
            {
                var inputCombListView = new PrintRuleInputCombineView();
                inputCombListView.DataContext = this;
                RegionManager.Regions[RegionNames.PrintRuleInputCombine].Add(inputCombListView, null, true);
            }

            if (SeqRuleList.Count <= 0 && InputRuleList.Count > 0)
                CanRepeatOfInput = true;
        }

        #endregion Constructor

        #region Functions

        public ICommand CloseCommand { get; private set; }
        public void CanClose()
        {
            if (!IsAbleToAction)
                DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "경고", "프린터 항목이 남아있습니다.");
        }

        public ICommand Refresh { get; private set; }
        public void ConvertRuleToText()
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
                        foreach (RuleMain r in UsingRuleList)
                        {
                            if (r.Name.Equals(rName))
                            {
                                wordList[i] = r.PrintValue;
                                break;
                            }
                        }
                    }
                }

                var changedText = string.Join("", wordList.ToArray());
                if (changedText.Length > pObj.MaxLength)
                    changedText = changedText.Substring(0, pObj.MaxLength);

                if (obj is TextObject)
                    (obj as TextObject).Text = changedText;
                else if (obj is BarcodeObject)
                    (obj as BarcodeObject).Text = changedText;
            }
        }

        #endregion Functions

        public void Print(PrintLabelView PLView)
        {
            try
            {
                switch (Label.SelectedPrinter)
                {
                    case PrinterOption.PrinterType.ZEBRA:
                        var zplCode = new System.Text.StringBuilder();
                        zplCode.AppendFormat("^XA");
                        zplCode.AppendFormat("^FO{0},{1}", Label.OffsetX, Label.OffsetY);
                        zplCode.AppendFormat("{0}", BitmapConversion.ConvertImageToZPLString(PLView));
                        zplCode.AppendFormat("^FS");
                        zplCode.AppendFormat("^XZ");
                        for (var i = 0; i < Label.NumberOfCopies; i++)
                        {
                            pService.PrintZebraProduct(SelectedPrinterName, zplCode.ToString());
                            //Test
                            Clipboard.Clear();
                            Clipboard.SetText(zplCode.ToString());
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "프린터 오류", ex.Source + ex.Message + "\n" + ex.StackTrace);
            }
        }

    }
}