using System.Windows;

namespace BasicModule.Views.Rule
{
    /// <summary>
    /// RuleManagerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RuleManagerWindow : Window
    {
        public RuleManagerWindow()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
