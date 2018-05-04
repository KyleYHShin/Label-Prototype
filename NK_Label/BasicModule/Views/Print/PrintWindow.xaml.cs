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

        #region Printing

        private PrintWindowViewModel pWinVM;
        private RuleSequentialNum rsn;
        private ulong currNum;
        private bool isTurnToPrint = false;
        private Timer timer;
        private void PrintLabel(object sender, RoutedEventArgs e)
        {
            switch (pWinVM.SeqRuleList.Count)
            {
                case 0:
                    pWinVM.IsAbleToUI = false;
                    for (var i = 0; i < pWinVM.Repetition; i++)
                    {
                        if (!ShowInputWindowAndGetCommand)
                            break;

                        pWinVM.ConvertRuleToText();
                        pWinVM.Print(PrintLabelView.Content as PrintLabelView);
                    }
                    pWinVM.IsAbleToUI = true;
                    break;
                case 1:
                    pWinVM.IsAbleToUI = false;
                    rsn = pWinVM.SeqRuleList[0].Content as RuleSequentialNum;
                    currNum = rsn.CurrNum;
                    isTurnToPrint = false;

                    timer = new Timer();
                    timer.Interval = 200;
                    timer.Tick += new System.EventHandler(Timer_Tick);
                    timer.Start();
                    break;
            }
        }
        private bool ShowInputWindowAndGetCommand
        {
            get
            {
                if (pWinVM.InputRuleList.Count > 0)
                {
                    foreach(var r in pWinVM.InputRuleList)
                    {
                        (r.Content as RuleInput).InputData = string.Empty;
                    }

                    var dlWin = new ReadyRuleInputListWindow
                    {
                        Title = "Ready to Manual Input"
                    };
                    dlWin.DataContext = pWinVM;
                    dlWin.pWinVM = pWinVM;
                    bool? ret = dlWin.ShowDialog();

                    return !ret.HasValue? false : (bool)ret;
                }
                return true;
            }
        }

        private void TerminateTimer()
        {
            rsn = null;
            currNum = 0;
            isTurnToPrint = false;

            if(timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }

            pWinVM.ConvertRuleToText();
            pWinVM.IsAbleToUI = true;
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            pWinVM.ConvertRuleToText();

            if (currNum > rsn.MaxNum)
                TerminateTimer();

            if (isTurnToPrint)
            {
                if (!ShowInputWindowAndGetCommand)
                    TerminateTimer();

                var content = PrintLabelView.Content;
                pWinVM.Print(content as PrintLabelView);
                currNum++;
                rsn.CurrNum = currNum;
                isTurnToPrint = false;
            }
            else
                isTurnToPrint = true;
        }

        private void Click_Stop(object sender, RoutedEventArgs e)
        {
            TerminateTimer();
        }

        #endregion Printing

        private void Click_Close(object sender, CancelEventArgs e)
        {
            if (!pWinVM.IsAbleToUI || timer != null)
            {
                Utils.DialogService.ShowSimpleTextDialog(this, "Warning", "프린터 항목이 남아있습니다.");
                e.Cancel = true;
            }
        }
            
    }
}