using BasicModule.Models.Rule;
using BasicModule.ViewModels.Print;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
            InputBox.Focus();
        }
        
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Tab)
            {
                if (noEmptyInputs)
                    DialogResult = true;
            }
        }

        private bool noEmptyInputs
        {
            get
            {
                if (string.IsNullOrEmpty(InputBox.Text))
                    return false;

                for (int i = 0; i < InputList.Items.Count; i++)
                {
                    var ri = (InputList.Items[i] as RuleMain).Content as RuleInput;
                    if (string.IsNullOrEmpty(ri.InputData))
                    {
                        ri.InputData = InputBox.Text;

                        if (i == InputList.Items.Count - 1)
                            return true;
                        else
                        {
                            InputBox.Text = string.Empty;
                            InputBox.Focus();
                            return false;
                        }
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
