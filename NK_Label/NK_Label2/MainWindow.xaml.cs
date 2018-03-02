using NK_Label2.Views;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Ink;

namespace NK_Label2
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

        void OnAddLabelInCanvas(object sender, RoutedEventArgs e)
        {
            LabelView lbView = new LabelView();
            mainWindow.Children.Add(lbView);
            //객체 변동 체크 -> 수동 드로잉
            int zindex = mainWindow.Children.Count;
            //Canvas.SetZIndex(lbView, zindex);
            Canvas.SetLeft(lbView, 200);
            Canvas.SetTop(lbView, 100);
        }

        void OnAddLabelInGrid(ICommand command)
        {
            LabelView lbView = new LabelView();
            mainWindow.Children.Add(lbView);
            int zindex = mainWindow.Children.Count;
            //Canvas.SetZIndex(lbView, zindex);
            Canvas.SetLeft(lbView, 0);
            Canvas.SetTop(lbView, 0);
            //+= LabelView;
        }


        private void SaveObject_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("Ink.xaml", XamlWriter.Save(subWindow.Strokes));
        }
        
        private void ClearObject_Click(object sender, RoutedEventArgs e)
        {
            subWindow.Strokes.Clear();
        }

        private void LoadObject_Click(object sender, RoutedEventArgs e)
        {
            subWindow.Strokes = (StrokeCollection)XamlReader.Load(File.OpenRead("Ink.xaml"));
        }
    }
}
