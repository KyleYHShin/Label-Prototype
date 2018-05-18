using NK_Label3.ViewModels;
using System.Windows;

namespace NK_Label3.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
