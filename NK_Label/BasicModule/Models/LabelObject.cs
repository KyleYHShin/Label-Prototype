using BasicModule.Models.Common;
using BasicModule.Models.Option;

namespace BasicModule.Models
{
    public class LabelObject : BasicObject
    {
        #region Vector Properties

        private double _width;
        public new double Width
        {
            get { return _width; }
            set
            {
                if (value >= 5)
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
                if (value >= 3)
                {
                    _height = getRound(value, 2);
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

        #endregion Vector Properties

        #region View Properties

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

        #endregion View Properties

        #region Print Properties

        private PrinterOption.PrinterType _selectedPrinter;
        public PrinterOption.PrinterType SelectedPrinter { get { return _selectedPrinter; } set { _selectedPrinter = value; OnPropertyChanged(); } }

        private double _selectedDpi;
        public double SelectedDpi
        {
            get { return _selectedDpi; }
            set
            {
                if (value > 0)
                {
                    _selectedDpi = value;
                    OnPropertyChanged();
                    ResetActualSize();
                }
            }
        }

        private ushort _offsetX;
        public ushort OffsetX { get { return _offsetX; } set { _offsetX = value; OnPropertyChanged(); } }

        private ushort _offsetY;
        public ushort OffsetY { get { return _offsetY; } set { _offsetY = value; } }

        private int _numberOfCopies = 1;
        public int NumberOfCopies { get { return _numberOfCopies; } set { _numberOfCopies = value; OnPropertyChanged(); } }


        private int _repeatOfInputs = 1;
        public int RepeatOfInputs { get { return _repeatOfInputs; } set { _repeatOfInputs = value; OnPropertyChanged(); } }

        private bool _enableSequentialInputs = false;
        public bool EnableSequentialInputs { get { return _enableSequentialInputs; } set { _enableSequentialInputs = value; OnPropertyChanged(); } }

        private int _serialNumberStartIndex;
        public int SerialNumberStartIndex
        {
            get { return _serialNumberStartIndex + 1; }
            set
            {
                if (value > 0)
                {
                    _serialNumberStartIndex = value - 1;
                    OnPropertyChanged();
                }
            }
        }

        private int _serialNumberLength = 1;
        public int SerialNumberLength
        {
            get { return _serialNumberLength; }
            set
            {
                if (value > 0)
                {
                    _serialNumberLength = value;
                    OnPropertyChanged();
                    LastSerialNumber = string.Empty.PadLeft(_serialNumberLength, '0');
                }
            }
        }

        private string _lastSerialNumber;
        public string LastSerialNumber
        {
            get
            {
                if (string.IsNullOrEmpty(_lastSerialNumber))
                    _lastSerialNumber = string.Empty.PadLeft(_serialNumberLength, '0');

                return _lastSerialNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _lastSerialNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Print Properties

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
