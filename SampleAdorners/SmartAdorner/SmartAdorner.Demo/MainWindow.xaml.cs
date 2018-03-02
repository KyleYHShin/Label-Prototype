using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SmartAdorner.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Icons = new ObservableCollection<Object>();
            for (int i = 0; i < 10; i++)
            {
                var text = "Complex Obj " + (i + 1);
                var icon = new IconViewModel()
                {
                    Text = text,
                    X = (i % 10 + 0.1) * 100 + 10,
                    Y = (i / 10) * 120 + 10
                };
                if (i > 7)
                {
                    icon.X = ((i - 5) % 10 + 0.2) * 200 + 10;
                    icon.Y = (i / 10 + 1.5) * 120 + 10;
                    icon.changeBarcodeFormat("DATA_MATRIX");
                }
                else if (i > 4)
                {
                    icon.X = ((i - 5) % 10 + 0.2) * 200 + 10;
                    icon.Y = (i / 10 + 1.5) * 120 + 10;
                    icon.changeBarcodeFormat("CODE_128");
                }

                Icons.Add(icon);
            }

            Icons.Add(new RectViewModel() { X = 300, Y = 300, Width = 200, Height = 100 });

            //Icons.Add(new LineViewModel() { X1 = 10, Y1 = 20, X2 = 100, Y2 = 100 });

            DataContext = this;
        }

        public ObservableCollection<Object> Icons { get; private set; }

        //private void IconThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        //{
        //    Thumb thumb = (Thumb)sender;
        //    IconViewModel icon = (IconViewModel)thumb.DataContext;
        //    icon.X += e.HorizontalChange;
        //    icon.Y += e.VerticalChange;
        //}
    }
}
