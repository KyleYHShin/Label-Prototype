using Program.ViewModels;
using System.Windows;

namespace Program.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MaxWidth = SystemParameters.PrimaryScreenWidth;
            MaxHeight = SystemParameters.PrimaryScreenHeight;
        }

        private void Click_Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel)
            {
                if ((DataContext as MainWindowViewModel).CanCloseAllLabel())
                    e.Cancel = false;
                else
                    e.Cancel = true;
            }
        }
    }
}
