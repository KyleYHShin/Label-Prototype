using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BasicModule.Models
{
    public class BasicObject : INotifyPropertyChanged
    {

        private bool _changed = false;
        public bool Changed
        {
            get { return _changed; }
            set
            {
                _changed = value;
                OnPropertyChanged();
            }
        }

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

        #region Vector Properties

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

        private double _posX = 0;
        public double PosX
        {
            get { return _posX; }
            set
            {
                _posX = getRound(value, 2);
                OnPropertyChanged();
            }
        }

        private double _posY = 0;
        public double PosY
        {
            get { return _posY; }
            set
            {
                _posY = getRound(value, 2);
                OnPropertyChanged();
            }
        }

        #endregion //Vector Properties

        protected double getRound(double original, int point)
        {
            return Math.Round(original, point);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
            if (propertyname != "Changed")
                Changed = true;
        }

        #endregion //INotifyPropertyChanged

    }
}
