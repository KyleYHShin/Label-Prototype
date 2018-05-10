using BasicModule.Common;
using BasicModule.Models.Rule;
using BasicModule.Models.Rule.Content;
using BasicModule.ViewModels.Print;
using Prism.Regions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace BasicModule.Views.Print
{
    /// <summary>
    /// PrintView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PrintWindow : Window
    {
        private PrintWindowViewModel thisDataContext;

        public PrintWindow(IRegionManager regionManager)
        {
            InitializeComponent();

            if (regionManager != null)
            {
                RegionController.SetRegionManager(regionManager, this.PrintRuleSeq, RegionNames.PrintRuleSeq);
                RegionController.SetRegionManager(regionManager, this.PrintRuleManu, RegionNames.PrintRuleManu);
                RegionController.SetRegionManager(regionManager, this.PrintRuleTime, RegionNames.PrintRuleTime);
                RegionController.SetRegionManager(regionManager, this.PrintRuleInput, RegionNames.PrintRuleInput);
                RegionController.SetRegionManager(regionManager, this.PrintRuleInputCombine, RegionNames.PrintRuleInputCombine);
            }

            SizeToContent = SizeToContent.WidthAndHeight;
            thisDataContext = DataContext as PrintWindowViewModel;
        }

        public void SetPrintLabelView(PrintLabelView plv)
        {
            PrintLabelView.Content = plv;
        }

        #region Input Printing

        private Timer inputTimer;
        bool isConverted = false, readyToPrint = false;
        int repeat = 0;
        private void InputTimer_Tick(object sender, System.EventArgs e)
        {
            if (isConverted && readyToPrint)
            {
                thisDataContext.Print(PrintLabelView.Content as PrintLabelView);
                repeat--;
                isConverted = false;
                readyToPrint = false;
                if (repeat <= 0)
                    TerminateInputTimer();
            } else if (isConverted && !readyToPrint) {
                readyToPrint = true;
            }
            else if (!isConverted && !readyToPrint)
            {
                inputTimer.Stop();
                if (!ShowInputWindowAndGetCommand)
                {
                    TerminateInputTimer();
                }
                else
                {
                    if (thisDataContext.InCombRuleList.Count > 0)
                    {
                        var combRule = thisDataContext.InCombRuleList[0].Content as RuleInputCombine;
                        foreach (var r in thisDataContext.InputRuleList)
                        {
                            combRule.AddInput((r.Content as RuleInput).InputData);
                        }
                    }

                    isConverted = true;
                    readyToPrint = false;
                    inputTimer.Start();
                }
            }
            thisDataContext.ConvertRuleToText();
        }

        private bool ShowInputWindowAndGetCommand
        {
            get
            {
                if (thisDataContext.InputRuleList.Count > 0)
                {
                    string tempLastSerialNumber = thisDataContext.Label.LastSerialNumber;

                    var tempInputRuleList = new List<IRuleObject>();
                    foreach (var r in thisDataContext.InputRuleList)
                    {
                        tempInputRuleList.Add(r.Content.Clone);
                        (r.Content as RuleInput).InputData = string.Empty;
                    }
                    IRuleObject tempInputCombRule = null;
                    if (thisDataContext.InCombRuleList.Count > 0)
                    {
                        tempInputCombRule = thisDataContext.InCombRuleList[0].Content.Clone;
                        (thisDataContext.InCombRuleList[0].Content as RuleInputCombine).InputRefresh();
                    }

                    var dlWin = new ReadyRuleInputListWindow
                    {
                        Title = "Ready to Manual Input",
                        DataContext = thisDataContext
                    };
                    dlWin.InitializeDataContext();
                    dlWin.Owner = System.Windows.Application.Current.MainWindow;
                    bool? ret = dlWin.ShowDialog();

                    if (!ret.HasValue || !(bool)ret)
                    {
                        thisDataContext.Label.LastSerialNumber = tempLastSerialNumber;
                        for(int i = 0; i<tempInputRuleList.Count; i++)
                            thisDataContext.InputRuleList[i].Content = tempInputRuleList[i].Clone;

                        if (tempInputCombRule != null)
                            thisDataContext.InCombRuleList[0].Content = tempInputCombRule;
                    }

                    return !ret.HasValue ? false : (bool)ret;
                }
                return true;
            }
        }

        private void TerminateInputTimer()
        {
            if (inputTimer != null)
            {
                inputTimer.Stop();
                inputTimer.Dispose();
                inputTimer = null;
            }

            isConverted = false;
            readyToPrint = false;
            repeat = 0;

            thisDataContext.ConvertRuleToText();
            thisDataContext.IsAbleToAction = true;
        }

        #endregion

        #region SeqPrinting

        private RuleSequentialNum rsn;
        private ulong currNum;
        private bool isTurnToPrint = false;
        private Timer seqTimer;

        private void SeqTimer_Tick(object sender, System.EventArgs e)
        {
            thisDataContext.ConvertRuleToText();

            if (currNum > rsn.MaxNum)
                TerminateSeqTimer();

            if (isTurnToPrint)
            {
                thisDataContext.Print(PrintLabelView.Content as PrintLabelView);
                currNum++;
                rsn.CurrNum = currNum;
                isTurnToPrint = false;
            }
            else
                isTurnToPrint = true;
        }

        private void TerminateSeqTimer()
        {
            if (seqTimer != null)
            {
                seqTimer.Stop();
                seqTimer.Dispose();
                seqTimer = null;
            }

            rsn = null;
            currNum = 0;
            isTurnToPrint = false;

            thisDataContext.ConvertRuleToText();
            thisDataContext.IsAbleToAction = true;
        }

        #endregion SeqPrinting



        private void PrintLabel(object sender, RoutedEventArgs e)
        {
            thisDataContext.ConvertRuleToText();

            bool hasSequentialRule = thisDataContext.SeqRuleList.Count > 0 ? true : false;
            bool hasInputRule = thisDataContext.InputRuleList.Count > 0 ? true : false;

            if (!hasSequentialRule && !hasInputRule)
            {
                thisDataContext.IsAbleToAction = false;
                for (var i = 0; i < thisDataContext.Repetition; i++)
                {
                    thisDataContext.Print(PrintLabelView.Content as PrintLabelView);
                }
                thisDataContext.IsAbleToAction = true;
            }
            else if (hasSequentialRule && !hasInputRule)
            {
                thisDataContext.IsAbleToAction = false;
                rsn = thisDataContext.SeqRuleList[0].Content as RuleSequentialNum;
                currNum = rsn.CurrNum;
                isTurnToPrint = false;

                seqTimer = new Timer();
                seqTimer.Interval = 200;
                seqTimer.Tick += new System.EventHandler(SeqTimer_Tick);
                seqTimer.Start();
            }
            else if (!hasSequentialRule && hasInputRule)
            {
                thisDataContext.IsAbleToAction = false;
                isConverted = false;
                readyToPrint = false;
                repeat = thisDataContext.Repetition;

                inputTimer = new Timer();
                inputTimer.Interval = 200;
                inputTimer.Tick += new System.EventHandler(InputTimer_Tick);
                inputTimer.Start();
            }
            else if (hasSequentialRule && hasInputRule)
            {
                thisDataContext.IsAbleToAction = false;
                isConverted = false;
                readyToPrint = false;
                rsn = thisDataContext.SeqRuleList[0].Content as RuleSequentialNum;
                currNum = rsn.CurrNum;

                multiTimer = new Timer();
                multiTimer.Interval = 200;
                multiTimer.Tick += new System.EventHandler(MultiTimer_Tick);
                multiTimer.Start();
            }
        }

        private Timer multiTimer;
        private void MultiTimer_Tick(object sender, System.EventArgs e)
        {
            if (isConverted && readyToPrint)
            {
                thisDataContext.Print(PrintLabelView.Content as PrintLabelView);
                repeat--;
                isConverted = false;
                readyToPrint = false;

                currNum++;
                rsn.CurrNum = currNum;
                if (currNum > rsn.MaxNum)
                    TerminateMultiTimer();
            }
            else if (isConverted && !readyToPrint)
            {
                readyToPrint = true;
            }
            else if (!isConverted && !readyToPrint)
            {
                multiTimer.Stop();
                if (!ShowInputWindowAndGetCommand)
                {
                    TerminateMultiTimer();
                }
                else
                {
                    if (thisDataContext.InCombRuleList.Count > 0)
                    {
                        var combRule = thisDataContext.InCombRuleList[0].Content as RuleInputCombine;
                        foreach (var r in thisDataContext.InputRuleList)
                        {
                            combRule.AddInput((r.Content as RuleInput).InputData);
                        }
                    }

                    isConverted = true;
                    readyToPrint = false;
                    multiTimer.Start();
                }
            }
            thisDataContext.ConvertRuleToText();
        }

        private void TerminateMultiTimer()
        {
            if (multiTimer != null)
            {
                multiTimer.Stop();
                multiTimer.Dispose();
                multiTimer = null;
            }

            isConverted = false;
            readyToPrint = false;
            rsn = null;
            currNum = 0;
            
            thisDataContext.ConvertRuleToText();
            thisDataContext.IsAbleToAction = true;
        }



        private void Click_Stop(object sender, RoutedEventArgs e)
        {
            TerminateSeqTimer();
            TerminateInputTimer();
            TerminateMultiTimer();
        }

        private void Click_Close(object sender, CancelEventArgs e)
        {
            if (!thisDataContext.IsAbleToAction || seqTimer != null)
            {
                Utils.DialogService.ShowSimpleTextDialog(this, "Warning", "프린터 항목이 남아있습니다.");
                e.Cancel = true;
            }
        }
            
    }
}