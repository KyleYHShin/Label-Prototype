using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Rule;
using BasicModule.Utils;
using BasicModule.Views.Print;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.ViewModels.Print
{
    public class PrintViewModel : BindableBase
    {
        #region Properties

        private LabelObject _label;
        public LabelObject Label { get { return _label; } set { SetProperty(ref _label, value); } }

        private ObservableCollection<BasicObject> _objectList;
        public ObservableCollection<BasicObject> ObjectList { get { return _objectList; } set { SetProperty(ref _objectList, value); } }

        private List<RuleMain> _ruleList;
        public List<RuleMain> RuleList { get { return _ruleList; } set { SetProperty(ref _ruleList, value); } }

        public int Repetition { get; set; } = 1;
        public List<string> _printerList;
        public List<string> PrinterList { get { return _printerList; } set { SetProperty(ref _printerList, value); } }

        private PrintService pService = new PrintService();
        private int _offsetX;
        public int OffsetX { get { return _offsetX; } set { SetProperty(ref _offsetX, value); } }
        private int _offsetY;
        public int OffsetY { get { return _offsetY; } set { SetProperty(ref _offsetY, value); } }

        #endregion

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

            PrinterList = pService.GetPrinterList();
            ConvertRuleToText();

            ClickStartPrint = new DelegateCommand(StartPrint);
        }

        private List<RuleSequentialNum> GetRuleSequentialNumList()
        {
            var rsnList = new List<RuleSequentialNum>();
            foreach (var obj in ObjectList)
            {
                if (obj is IPrintableObject)
                {
                    var pObj = obj as IPrintableObject;
                    if (!string.IsNullOrEmpty(pObj.OriginText))
                    {
                        var wordList = RuleRregulation.RuleNameSeperatorToList(pObj.OriginText);
                        foreach (var word in wordList)
                        {
                            if (RuleRregulation.RuleNameVerifier(word))
                            {
                                foreach (RuleMain r in RuleList)
                                {
                                    if (r.Name.Equals(word) && r.Format == RuleRregulation.RuleFormat.SEQUENTIAL_NUM)
                                    {
                                        var rsn = r.Content as RuleSequentialNum;
                                        if (!rsnList.Contains(rsn))
                                        {
                                            rsnList.Add(rsn);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rsnList;
        }

        public ICommand ClickStartPrint { get; private set; }
        public void StartPrint()
        {
            List<RuleSequentialNum> rsnList = GetRuleSequentialNumList();
            if (rsnList.Count == 0)
            {
                for (var i = 0; i < Repetition; i++)
                {
                    ConvertRuleToText();
                    //print to window
                    //screenshot
                    //print
                }
            }
            else if (rsnList.Count == 1)
            {
                RuleSequentialNum rsn = rsnList[0];
                for (var i = rsn.CurrNum; i <= rsn.MaxNum; i += rsn.Increment)
                {
                    rsn.CurrNum = i;
                    ConvertRuleToText();
                    //print to window
                    //screen shot
                    //print

                    ////이미지화
                    //Bitmap img = null;
                    //string[] oList = null;
                    //bool ret = Utils.PrintService.PrintLabel(img, oList);
                    var ps = new PrintService();
                    ps.Label = Label;
                    ps.ObjectList = ObjectList;
                    ps.PrintLabel();
                }
            }
            else
            {
                // 오류! -> Dialog window
            }
        }

        private void ConvertRuleToText()
        {
            foreach (var obj in ObjectList)
            {
                if (obj is IPrintableObject)
                {
                    var pObj = obj as IPrintableObject;
                    if (!string.IsNullOrEmpty(pObj.OriginText))
                    {
                        var wordList = RuleRregulation.RuleNameSeperatorToList(pObj.OriginText);

                        for (int i = 0; i < wordList.Count; i++)
                        {
                            if (RuleRregulation.RuleNameVerifier(wordList[i]))
                            {
                                var rName = RuleRregulation.RuleNameExtractor(wordList[i]);
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
            }
        }

        public void TestZebraPrint(PrintLabelView PLView)
        {
            try
            {
                var ZPLCommand = new System.Text.StringBuilder();

                ZPLCommand.AppendFormat("^XA");
                ZPLCommand.AppendFormat("^FO{0},{1}", OffsetX, OffsetY);
                ZPLCommand.AppendFormat("{0}", BitmapConversion.ConvertImageToZPLString(PLView));
                ZPLCommand.AppendFormat("^FS");
                ZPLCommand.AppendFormat("^XZ");

                Console.WriteLine(ZPLCommand);
                pService.PrintZebraProduct("", ZPLCommand.ToString());
            }
            catch (Exception ex)
            {
                new DialogService().ShowSimpleTextDialog(Application.Current.MainWindow, "Exception", ex.Source + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
