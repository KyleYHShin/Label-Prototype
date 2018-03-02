using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NK_Label2.Models
{
    public class TextModel : INotifyPropertyChanged
    {
        #region Property Handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion //Property Handler

        #region Object Default
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != null)
                {
                    _Name = value;
                    OnPropertyChanged("Content");
                }
            }
        }
        #endregion //Object Default

        #region Vector Data
        private Point _Point = new Point()
        {
            X = 10,
            Y = 10
        };
        private double _PosX = 0;
        private double _PosY = 0;
        private double _Width = 100;
        private double _Height = 30;

        public Point Point
        {
            get { return _Point; }
            set
            {
                _Point.X = value.X;
                _Point.Y = value.Y;
                OnPropertyChanged("Point");
            }
        }
        public double PosX
        {
            get { return _PosX; }
            set
            {
                _PosX = value;
                OnPropertyChanged("PosX");
            }
        }
        public double PosY
        {
            get { return _PosY; }
            set
            {
                _PosY = value;
                OnPropertyChanged("PosY");
            }
        }
        public double Width
        {
            get { return _Width; }
            set
            {
                if (value > 0)
                {
                    _Width = value;
                    OnPropertyChanged("Width");
                }
            }
        }
        public double Height
        {
            get { return _Height; }
            set
            {
                if (value > 0)
                {
                    _Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }
        #endregion //Vector Data

        #region Text Data
        private TextBlock tbl = new TextBlock()
        {
            Text = "TextBox",
            FontSize = 15
        };
        public string Text
        {
            get { return tbl.Text; }
            set
            {
                if (value != null)
                {
                    tbl.Text = value;
                    OnPropertyChanged("Text");
                }
            }
        }
        public double FontSize
        {
            get { return tbl.FontSize; }
            set
            {
                if (value >= 0)
                {
                    tbl.FontSize = value;
                    OnPropertyChanged("FontSize");
                }
            }
        }
        public FontFamily FontFamily
        {
            get { return tbl.FontFamily; }
            set
            {
                if (value != null)
                {
                    tbl.FontFamily = value;
                    OnPropertyChanged("FontFamily");
                }
            }
        }
        #endregion //Text Data
    }
}
