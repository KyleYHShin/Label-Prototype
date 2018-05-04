using BasicModule.Models.Rule;
using BasicModule.ViewModels.Print;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.Views.Print
{
    /// <summary>
    /// ReadyRuleInputListWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReadyRuleInputListWindow : Window
    {
        public ReadyRuleInputListWindow()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        public PrintWindowViewModel pWinVM { get; set; }
        
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            var obj = sender; //check

            if (e.Key == Key.Return || e.Key == Key.Tab)
            {
                if (IsNoEmptyInput)
                    DialogResult = true;
            }
        }

        private bool IsNoEmptyInput
        {
            get
            {
                foreach (var r in pWinVM.InputRuleList)
                {
                    if (string.IsNullOrEmpty((r.Content as RuleInput).InputData))
                    {
                        //해당 칸에 커서 이동
                        return false;
                    }
                }
                return true;
            }
        }

        private void Click_Print(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
