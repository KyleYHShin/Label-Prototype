using System.Windows;
using System.Windows.Controls;

using BasicModule.ViewModels.Option;

namespace BasicModule.Views.Common
{
    /// <summary>
    /// ChildWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DialogSelectWindow : Window
    {
        public DialogSelectWindow()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var content = PART_ContentControl.Content;
            if (content is UserControl)
            {
                var dataContext = (content as UserControl).DataContext;
                if (dataContext is IOptionViewModel)
                {
                    if ((dataContext as IOptionViewModel).isRight())
                        DialogResult = true;
                }
                else
                    DialogResult = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
