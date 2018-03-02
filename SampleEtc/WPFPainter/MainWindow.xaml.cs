using System.Windows;
using System.Windows.Markup;
using System.IO;
using System.Windows.Ink;

namespace WpfApplication6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Strokes.Clear();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("Ink.xaml", XamlWriter.Save(Canvas.Strokes));
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Strokes = (StrokeCollection)XamlReader.Load(File.OpenRead("Ink.xaml"));
        }
    }
}
