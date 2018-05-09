using BasicModule.Common;
using BasicModule.Models.Rule;
using BasicModule.ViewModels.Print;
using Prism.Regions;
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
        private PrintWindowViewModel pWinVM;

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
            pWinVM = DataContext as PrintWindowViewModel;
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
                pWinVM.Print(PrintLabelView.Content as PrintLabelView);
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
                    isConverted = true;
                    readyToPrint = false;
                    inputTimer.Start();
                }
            }
            pWinVM.ConvertRuleToText();
        }

        private bool ShowInputWindowAndGetCommand
        {
            get
            {
                if (pWinVM.InputRuleList.Count > 0)
                {
                    foreach (var r in pWinVM.InputRuleList)
                    {
                        (r.Content as RuleInput).InputData = string.Empty;
                    }

                    var dlWin = new ReadyRuleInputListWindow
                    {
                        Title = "Ready to Manual Input",
                        DataContext = pWinVM
                    };

                    dlWin.Owner = System.Windows.Application.Current.MainWindow;
                    bool? ret = dlWin.ShowDialog();

                    return !ret.HasValue ? false : (bool)ret;
                }
                return true;
            }
        }

        private void TerminateInputTimer()
        {
            inputTimer.Dispose();
            inputTimer = null;

            isConverted = false;
            readyToPrint = false;
            repeat = 0;

            pWinVM.IsAbleToUI = true;
        }

        #endregion

        #region SeqPrinting

        private RuleSequentialNum rsn;
        private ulong currNum;
        private bool isTurnToPrint = false;
        private Timer seqTimer;

        private void TerminateSeqTimer()
        {
            rsn = null;
            currNum = 0;
            isTurnToPrint = false;

            if (seqTimer != null)
            {
                seqTimer.Stop();
                seqTimer.Dispose();
                seqTimer = null;
            }

            pWinVM.ConvertRuleToText();
            pWinVM.IsAbleToUI = true;
        }

        private void SeqTimer_Tick(object sender, System.EventArgs e)
        {
            pWinVM.ConvertRuleToText();

            if (currNum > rsn.MaxNum)
                TerminateSeqTimer();

            if (isTurnToPrint)
            {
                pWinVM.Print(PrintLabelView.Content as PrintLabelView);
                currNum++;
                rsn.CurrNum = currNum;
                isTurnToPrint = false;
            }
            else
                isTurnToPrint = true;
        }

        #endregion SeqPrinting



        private void PrintLabel(object sender, RoutedEventArgs e)
        {
            pWinVM.ConvertRuleToText();

            bool hasSequentialRule = pWinVM.SeqRuleList.Count > 0 ? true : false;
            bool hasInputRule = pWinVM.InputRuleList.Count > 0 ? true : false;

            if (!hasSequentialRule && !hasInputRule)
            {
                pWinVM.IsAbleToUI = false;
                for (var i = 0; i < pWinVM.Repetition; i++)
                {
                    pWinVM.Print(PrintLabelView.Content as PrintLabelView);
                }
                pWinVM.IsAbleToUI = true;
            }
            else if (hasSequentialRule && !hasInputRule)
            {
                pWinVM.IsAbleToUI = false;
                rsn = pWinVM.SeqRuleList[0].Content as RuleSequentialNum;
                currNum = rsn.CurrNum;
                isTurnToPrint = false;

                seqTimer = new Timer();
                seqTimer.Interval = 200;
                seqTimer.Tick += new System.EventHandler(SeqTimer_Tick);
                seqTimer.Start();
            }
            else if (!hasSequentialRule && hasInputRule)
            {
                pWinVM.IsAbleToUI = false;
                isConverted = false;
                readyToPrint = false;
                repeat = pWinVM.Repetition;

                inputTimer = new Timer();
                inputTimer.Interval = 200;
                inputTimer.Tick += new System.EventHandler(InputTimer_Tick);
                inputTimer.Start();
            }
            else if (hasSequentialRule && hasInputRule)
            {
                pWinVM.IsAbleToUI = false;
                isConverted = false;
                readyToPrint = false;
                rsn = pWinVM.SeqRuleList[0].Content as RuleSequentialNum;
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
                pWinVM.Print(PrintLabelView.Content as PrintLabelView);
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
                    isConverted = true;
                    readyToPrint = false;
                    multiTimer.Start();
                }
            }
            pWinVM.ConvertRuleToText();
        }

        private void TerminateMultiTimer()
        {
            multiTimer.Dispose();
            multiTimer = null;

            isConverted = false;
            readyToPrint = false;
            rsn = null;
            currNum = 0;

            pWinVM.IsAbleToUI = true;
        }



        private void Click_Stop(object sender, RoutedEventArgs e)
        {
            TerminateSeqTimer();
        }

        private void Click_Close(object sender, CancelEventArgs e)
        {
            if (!pWinVM.IsAbleToUI || seqTimer != null)
            {
                Utils.DialogService.ShowSimpleTextDialog(this, "Warning", "프린터 항목이 남아있습니다.");
                e.Cancel = true;
            }
        }
            
    }
}