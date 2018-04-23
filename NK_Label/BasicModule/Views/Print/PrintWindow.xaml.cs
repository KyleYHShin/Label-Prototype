using BasicModule.ViewModels.Print;
using System.Windows;

namespace BasicModule.Views.Print
{
    /// <summary>
    /// PrintView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PrintWindow : Window
    {
        public PrintWindow(PrintLabelView plv)
        {
            InitializeComponent();
            LABEL_ContentControl.Content = plv;
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        public void PrintLabel(object sender, RoutedEventArgs e)
        {
            var content = LABEL_ContentControl.Content;
            if (DataContext is PrintViewModel && content is PrintLabelView)
            {
                (DataContext as PrintViewModel).TestZebraPrint(content as PrintLabelView);
            }
        }

        private void Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
