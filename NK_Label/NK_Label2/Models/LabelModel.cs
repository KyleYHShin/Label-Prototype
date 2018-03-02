using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NK_Label2.Models
{
    public class LabelModel : INotifyPropertyChanged
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
        private int _Printer;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != null)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public int Printer
        {
            get { return _Printer; }
            set
            {
                if (value >= 0)
                {
                    _Printer = value;
                    OnPropertyChanged("Printer");
                }
            }
        }
        #endregion //Object Default


        #region Vector Data
        private double _Width;
        private double _Height;

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

        #region Label Data
        //Border Radius settings
        #endregion Label Data

    }
}
