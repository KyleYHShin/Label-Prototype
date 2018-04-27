using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Option;
using BasicModule.Models.Rule;
using BasicModule.Utils;
using BasicModule.Views.Print;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace BasicModule.ViewModels.Print
{
    public class PrintViewModel : BindableBase
    {
        #region Label Properties

        private LabelObject _label;
        public LabelObject Label { get { return _label; } set { SetProperty(ref _label, value); } }

        private ObservableCollection<BasicObject> _objectList;
        public ObservableCollection<BasicObject> ObjectList { get { return _objectList; } set { SetProperty(ref _objectList, value); } }

        private List<RuleMain> _ruleList;
        public List<RuleMain> RuleList { get { return _ruleList; } set { SetProperty(ref _ruleList, value); } }

        private List<RuleSequentialNum> UsingRuleSequentialNumList;

        #endregion Label Properties

        #region Printer Properties

        private PrintService pService;

        private bool _repetable;
        public bool Repetable { get { return _repetable; } set { SetProperty(ref _repetable, value); } }

        private int _repetition = 1;
        public int Repetition { get { return _repetition; } set { SetProperty(ref _repetition, value); } }

        public List<string> _printerList;
        public List<string> PrinterList { get { return _printerList; } set { SetProperty(ref _printerList, value); } }

        private string _selectedPrinterName;
        public string SelectedPrinterName { get { return _selectedPrinterName; } set { SetProperty(ref _selectedPrinterName, value); } }
        
        private int _offsetX;
        public int OffsetX { get { return _offsetX; } set { SetProperty(ref _offsetX, value); } }
        private int _offsetY;
        public int OffsetY { get { return _offsetY; } set { SetProperty(ref _offsetY, value); } }

        #endregion Printer Properties

        #region Constructor

        public PrintViewModel(LabelObject label, ObservableCollection<BasicObject> objectList, List<RuleMain> ruleList)
        {
            Label = label; //swallow copy
            ObjectList = new ObservableCollection<BasicObject>(); //deep copy
            foreach (var obj in objectList)
            {
                if (obj is IPrintableObject)
                {
                    IPrintableObject newObj = (obj as IPrintableObject).Clone;
                    newObj.OriginText = newObj.Text;
                    newObj.Text = "";
                    ObjectList.Add(newObj as BasicObject);
                }
            }
            RuleList = ruleList; //swallow copy

            pService = new PrintService();
            PrinterList = pService.GetPrinterList();
            if (PrinterList.Count > 0)
                SelectedPrinterName = PrinterList[0];
            SelectedPrinterName = "ZDesigner GT800-300dpi EPL";// Test

            UsingRuleSequentialNumList = GetRuleSequentialNumList();
            if (UsingRuleSequentialNumList.Count > 0)
                Repetable = false;
            else
                Repetable = true;

            ConvertRuleToText();
        }

        #endregion Constructor

        #region Functions

        private List<RuleSequentialNum> GetRuleSequentialNumList()
        {
            var rsnList = new List<RuleSequentialNum>();
            foreach (var obj in ObjectList)
            {
                if (!(obj is IPrintableObject))
                    continue;

                var pObj = obj as IPrintableObject;
                if (string.IsNullOrEmpty(pObj.OriginText))
                    continue;

                var rsn = GetRuleSequentialNum(pObj.OriginText);
                if (rsn != null && !rsnList.Contains(rsn))
                    rsnList.Add(rsn);
            }
            return rsnList;
        }

        private RuleSequentialNum GetRuleSequentialNum(string originText)
        {
            var wordList = RuleRegulation.RuleNameSeperatorToList(originText);

            foreach (var word in wordList)
            {
                if (!RuleRegulation.RuleNameVerifier(word))
                    continue;

                var rName = RuleRegulation.RuleNameExtractor(word);
                foreach (RuleMain r in RuleList)
                {
                    if (r.Name.Equals(rName) && r.Format == RuleRegulation.RuleFormat.SEQUENTIAL_NUM)
                        return r.Content as RuleSequentialNum;
                }
            }
            return null;
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

        #endregion Functions
        
        #region Print

        public void StartPrint(PrintLabelView PLView)
        {
            try
            {
                switch (UsingRuleSequentialNumList.Count)
                {
                    case 0:
                        for (var i = 0; i < Repetition; i++)
                        {
                            ConvertRuleToText();
                            System.Threading.Thread.Sleep(1000);
                            Print(PLView);
                        }
                        break;
                    case 1:
                        RuleSequentialNum rsn = UsingRuleSequentialNumList[0];
                        for (var i = rsn.CurrNum; i <= rsn.MaxNum; i += rsn.Increment)
                        {
                            rsn.CurrNum = i;
                            ConvertRuleToText();
                            System.Threading.Thread.Sleep(1000);
                            Print(PLView);
                        }
                        break;
                    default:
                        new DialogService().ShowSimpleTextDialog(Application.Current.MainWindow, "Exception", "There are a lot of Serial Rules.");
                        break;
                }
            }
            catch (Exception ex)
            {
                new DialogService().ShowSimpleTextDialog(Application.Current.MainWindow, "Exception", ex.Source + ex.Message + "\n" + ex.StackTrace);
            }
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
                    pService.PrintZebraProduct(SelectedPrinterName, zplCode.ToString());
                    break;
            }
        }

        #endregion Print
    }
}
