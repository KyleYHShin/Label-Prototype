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
        #region Label Properties

        private LabelObject _label;
        public LabelObject Label { get { return _label; } set { SetProperty(ref _label, value); } }

        private ObservableCollection<BasicObject> _objectList;
        public ObservableCollection<BasicObject> ObjectList { get { return _objectList; } set { SetProperty(ref _objectList, value); } }
        
        private List<RuleMain> UsingRuleList { get; set; }

        private ObservableCollection<RuleMain> _seqRuleList;
        public ObservableCollection<RuleMain> SeqRuleList { get { return _seqRuleList; } set { SetProperty(ref _seqRuleList, value); } }

        private ObservableCollection<RuleMain> _manuRuleList;
        public ObservableCollection<RuleMain> ManuRuleList { get { return _manuRuleList; } set { SetProperty(ref _manuRuleList, value); } }

        private ObservableCollection<RuleMain> _timeRuleList;
        public ObservableCollection<RuleMain> TimeRuleList { get { return _timeRuleList; } set { SetProperty(ref _timeRuleList, value); } }

        private ObservableCollection<RuleMain> _inputRuleList;
        public ObservableCollection<RuleMain> InputRuleList { get { return _inputRuleList; } set { SetProperty(ref _inputRuleList, value); } }

        private ObservableCollection<RuleMain> _inComRuleList;
        public ObservableCollection<RuleMain> InCombRuleList { get { return _inComRuleList; } set { SetProperty(ref _inComRuleList, value); } }
        
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

        private bool _isAbleToUI = true;
        public bool IsAbleToUI
        {
            get { return _isAbleToUI; }
            set
            {
                SetProperty(ref _isAbleToUI, value);
                if (value)
                {
                    if (SeqRuleList.Count > 0)
                        Repetable = false;
                    else
                        Repetable = true;
                }
            }
        }

        #endregion Printer Properties

        #region Constructor

        private readonly IRegionManager RegionManager;
        public PrintWindowViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;

            Refresh = new DelegateCommand(ConvertRuleToText);
            CloseCommand = new DelegateCommand(CanClose);
        }

        public ICommand CloseCommand { get; private set; }
        public void CanClose()
        {
            if(!IsAbleToUI)
                DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "Warning","프린터 항목이 남아있습니다.");
            // how to prevent close window
        }

        #endregion Constructor

        #region Functions

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
                    newObj.Text = "";
                    ObjectList.Add(newObj as BasicObject);
                }
            }

            InitializeRuleData(originalRuleList);
            
            if (SeqRuleList.Count > 1 || InCombRuleList.Count > 1)
                return false;

            switch (SeqRuleList.Count)
            {
                case 0:
                    Repetable = true;
                    break;
                case 1:
                    Repetable = false;
                    break;
            }

            ConvertRuleToText();

            pService = new PrintService();
            UsablePrinterList = pService.GetUsablePrinterList();
            if (UsablePrinterList.Count > 0)
                SelectedPrinterName = UsablePrinterList[0];

            return true;
        }

        private void InitializeRuleData(List<RuleMain> originalRuleList)
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
            InputRuleList = new ObservableCollection<RuleMain>();
            var sortingInputRuleList = new List<RuleMain>();
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
                    case RuleRegulation.RuleFormat.INPUT:
                        sortingInputRuleList.Add(r);
                        break;
                    case RuleRegulation.RuleFormat.INPUT_COMBINE:
                        InCombRuleList.Add(r);
                        break;
                }
            }

            sortingInputRuleList.Sort((x, y) => (x.Content as RuleInput).Order.CompareTo((y.Content as RuleInput).Order));
            foreach(var r in sortingInputRuleList)
            {
                InputRuleList.Add(r);
            }

            RegionManager.Regions[RegionNames.PrintRuleSeq].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleManu].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleTime].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleInput].RemoveAll();
            RegionManager.Regions[RegionNames.PrintRuleInputCombine].RemoveAll();

            var seqListView = new PrintRuleSequentialNumView();
            seqListView.DataContext = this;
            RegionManager.Regions[RegionNames.PrintRuleSeq].Add(seqListView, null, true);

            var manuListView = new PrintRuleManualListView();
            manuListView.DataContext = this;
            RegionManager.Regions[RegionNames.PrintRuleManu].Add(manuListView, null, true);

            var timeListView = new PrintRuleTimeView();
            timeListView.DataContext = this;
            RegionManager.Regions[RegionNames.PrintRuleTime].Add(timeListView, null, true);

            var inputListView = new PrintRuleInputView();
            inputListView.DataContext = this;
            RegionManager.Regions[RegionNames.PrintRuleInput].Add(inputListView, null, true);

            //var inputCombListView = new PrintRuleInputCombineView();
            //inputCombListView.DataContext = this;
            //RegionManager.Regions[RegionNames.PrintRuleInputCombine].Add(inputCombListView, null, true);
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
                        zplCode.AppendFormat("^FO{0},{1}", OffsetX, OffsetY);
                        zplCode.AppendFormat("{0}", BitmapConversion.ConvertImageToZPLString(PLView));
                        zplCode.AppendFormat("^FS");
                        zplCode.AppendFormat("^XZ");

                        Console.WriteLine(zplCode);
                        //pService.PrintZebraProduct(SelectedPrinterName, zplCode.ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowSimpleTextDialog(Application.Current.MainWindow, "Exception", ex.Source + ex.Message + "\n" + ex.StackTrace);
            }
        }

    }
}