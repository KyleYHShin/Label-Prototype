using BasicModule.Common;
using BasicModule.ViewModels.Print;
using Prism.Regions;
using System.Windows;

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
                RegionController.SetRegionManager(regionManager, this.PrintLabelView, RegionNames.PrintLabelView);
                RegionController.SetRegionManager(regionManager, this.PrintRuleSeq, RegionNames.PrintRuleSeq);
                RegionController.SetRegionManager(regionManager, this.PrintRuleManu, RegionNames.PrintRuleManu);
                RegionController.SetRegionManager(regionManager, this.PrintRuleTime, RegionNames.PrintRuleTime);
            }

            SizeToContent = SizeToContent.WidthAndHeight;
        }

        public void SetPrintLabelView(PrintLabelView plv)
        {
            PrintLabelView.Content = plv;
        }

        public void PrintLabel(object sender, RoutedEventArgs e)
        {
            var content = PrintLabelView.Content;
            if (DataContext is PrintWindowViewModel && content is PrintLabelView)
                (DataContext as PrintWindowViewModel).StartPrint(content as PrintLabelView);
        }

        private void Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
