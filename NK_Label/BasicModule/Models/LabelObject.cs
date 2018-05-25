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
                _width = ValidWidth(getRound(value, 2));
                ResetActualSize();
                OnPropertyChanged();
            }
        }

        private double _height;
        public new double Height
        {
            get { return _height; }
            set
            {
                _height = ValidHeight(getRound(value, 2));
                ResetActualSize();
                OnPropertyChanged();
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

        private double ValidWidth(double value)
        {
            double minWidth = 5;
            if (value < minWidth)
                return minWidth;

            double minWidthLow = 225;
            double minWidthMid = 168;
            double minWidthHigh = 135;

            switch (SelectedDpi)
            {
                case PrinterOption.DPI_LOW:
                    if (value > minWidthLow)
                        return minWidthLow;
                    break;
                case PrinterOption.DPI_MID:
                    if (value > minWidthMid)
                        return minWidthMid;
                    break;
                case PrinterOption.DPI_HIGH:
                    if (value > minWidthHigh)
                        return minWidthHigh;
                    break;
            }
            return value;
        }

        private double ValidHeight(double value)
        {
            double minHeight = 3;
            if (value < minHeight)
                return minHeight;

            double minHeightLow = 168;
            double minHeightMid = 125;
            double minHeightHigh = 100;

            switch (SelectedDpi)
            {
                case PrinterOption.DPI_LOW:
                    if (value > minHeightLow)
                        return minHeightLow;
                    break;
                case PrinterOption.DPI_MID:
                    if (value > minHeightMid)
                        return minHeightMid;
                    break;
                case PrinterOption.DPI_HIGH:
                    if (value > minHeightHigh)
                        return minHeightHigh;
                    break;
            }
            return value;
        }

        private void ResetActualSize()
        {
            ConvertedWidth = Width * 10 * SelectedDpi;
            ConvertedHeight = Height * 10 * SelectedDpi;
            ConvertedGuideWidth = ConvertedWidth - (Margin * 10 * SelectedDpi * 2);
            ConvertedGuideHeight = ConvertedHeight - (Margin * 10 * SelectedDpi * 2);
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
                    Width = ValidWidth(Width);
                    Height = ValidHeight(Height);

                    ResetActualSize();
                    OnPropertyChanged();
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
