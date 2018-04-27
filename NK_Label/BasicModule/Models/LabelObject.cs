using BasicModule.Models.Common;
using BasicModule.Models.Option;

namespace BasicModule.Models
{
    public class LabelObject : BasicObject
    {
        #region Default Properties

        private PrinterOption.PrinterType _selectedPrinter;
        public PrinterOption.PrinterType SelectedPrinter { get { return _selectedPrinter; } set { _selectedPrinter = value; OnPropertyChanged(); } }

        private double _selectedDpi;
        public double SelectedDpi
        {
            get { return _selectedDpi; }
            set
            {
                _selectedDpi = value;
                OnPropertyChanged();
                ResetActualSize();
            }
        }

        #endregion Default Properties

        #region Size

        private double _width;
        public new double Width
        {
            get { return _width; }
            set
            {
                if (value > 0)
                {
                    _width = getRound(value, 2);
                    OnPropertyChanged();
                    ResetActualSize();
                }
            }
        }

        private double _height;
        public new double Height
        {
            get { return _height; }
            set
            {
                if (value > 0)
                {
                    _height = getRound(value, 2);
                    OnPropertyChanged();
                    ResetActualSize();
                }
            }
        }

        private int _margin = 10;
        public int Margin
        {
            get { return _margin; }
            set
            {
                if (value >= 0)
                {
                    _margin = value;
                    OnPropertyChanged();
                    ResetActualSize();
                }
            }
        }


        private double _convertedWidth;
        public double ConvertedWidth { get { return _convertedWidth; } set { _convertedWidth = value; OnPropertyChanged(); } }

        private double _convertedHeight;
        public double ConvertedHeight { get { return _convertedHeight; } set { _convertedHeight = value; OnPropertyChanged(); } }

        private double _convertedGuideWidth;
        public double ConvertedGuideWidth { get { return _convertedGuideWidth; } set { _convertedGuideWidth = value; OnPropertyChanged(); } }

        private double _convertedGuideHeight;
        public double ConvertedGuideHeight { get { return _convertedGuideHeight; } set { _convertedGuideHeight = value; OnPropertyChanged(); } }

        private void ResetActualSize()
        {
            ConvertedWidth = Width * 10 * SelectedDpi;
            ConvertedHeight = Height * 10 * SelectedDpi;
            ConvertedGuideWidth = (Width * 10 - Margin * 2) * SelectedDpi;
            ConvertedGuideHeight = (Height * 10 - Margin * 2) * SelectedDpi;
        }

        #endregion Size

        #region Grid Properties

        private double _viewSize = 1;
        public double ViewSize
        {
            get { return _viewSize; }
            set
            {
                if (value > 0)
                {
                    _viewSize = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private double _radius = 10;
        public double Radius
        {
            get { return _radius; }
            set
            {
                if (value >= 0)
                {
                    _radius = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Grid Properties

        public LabelObject Clone
        {
            get
            {
                LabelObject obj = new LabelObject();
                obj.Name = Name;
                obj.Width = Width;
                obj.Height = Height;
                obj.SelectedPrinter = SelectedPrinter;
                obj.SelectedDpi = SelectedDpi;
                obj.Radius = Radius;
                return obj;
            }
        }
    }
}
