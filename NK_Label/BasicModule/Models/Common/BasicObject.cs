using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BasicModule.Models.Common
{
    public class BasicObject : INotifyPropertyChanged
    {
        private bool _isChanged = false;
        public bool IsChanged { get { return _isChanged; } set { _isChanged = value; OnPropertyChanged(); } }

        private bool _isSelected;
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged(); } }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                if (value > 0)
                {
                    _width = getRound(value, 2);
                    OnPropertyChanged();
                }
            }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                if (value > 0)
                {
                    _height = getRound(value, 2);
                    OnPropertyChanged();
                }
            }
        }

        private double _posX = 10;
        public double PosX
        {
            get { return _posX; }
            set
            {
                if (value > 0)
                {
                    _posX = getRound(value, 2);
                    OnPropertyChanged();
                }
            }
        }

        private double _posY = 10;
        public double PosY
        {
            get { return _posY; }
            set
            {
                if (value > 0)
                {
                    _posY = getRound(value, 2);
                    OnPropertyChanged();
                }
            }
        }

        protected double getRound(double original, int point)
        {
            return Math.Round(original, point);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
            if (propertyname != "IsChanged" && propertyname != "IsSelected")
                IsChanged = true;
        }

    }
}
