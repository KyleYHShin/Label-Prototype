using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAdorner.Demo
{
    public class RectViewModel : INotifyPropertyChanged
    {
        public bool _isSelected;
        public bool IsSelected {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
                if (value)
                    Stroke = "Red";
                else
                    Stroke = "Transparent";
            }
        }

        private double m_X;
        public double X
        {
            get { return m_X; }
            set { m_X = value; OnPropertyChanged("X"); }
        }

        private double m_Y;
        public double Y
        {
            get { return m_Y; }
            set { m_Y = value; OnPropertyChanged("Y"); }
        }

        private double m_Width;
        public double Width
        {
            get { return m_Width; }
            set { m_Width = value; OnPropertyChanged("Width"); }
        }

        private double m_Height;
        public double Height
        {
            get { return m_Height; }
            set { m_Height = value; OnPropertyChanged("Height"); }
        }

        private string _stroke = "Transparent";
        public string Stroke
        {
            get { return _stroke; }
            set { _stroke = value; OnPropertyChanged("Stroke"); }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
