using System.Windows;

namespace BasicModule.Views.Common
{
    /// <summary>
    /// DialogWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
