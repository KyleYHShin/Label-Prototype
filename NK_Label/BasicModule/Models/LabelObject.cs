using BasicModule.Models.Common;
using BasicModule.Models.Option;

namespace BasicModule.Models
{
    public class LabelObject : BasicObject
    {
        #region Vector Properties

        private double _width = LabelOption.DEFAULT_WIDTH;
        public new double Width
        {
            get { return _width; }
            set
            {
                _width = ValidWidth(getRound(value, 2));
                OnPropertyChanged();
                ResetActualSize();
            }
        }

        private double _height = LabelOption.DEFAULT_HEIGHT;
        public new double Height
        {
            get { return _height; }
            set
            {
                _height = ValidHeight(getRound(value, 2));
                OnPropertyChanged();
                ResetActualSize();
            }
        }

        private double _convertedWidth = LabelOption.DEFAULT_WIDTH * PrinterOption.DEFAULT_DPI;
        public double ConvertedWidth { get { return _convertedWidth; } set { _convertedWidth = value; OnPropertyChanged(); } }

        private double _convertedHeight = LabelOption.DEFAULT_HEIGHT * PrinterOption.DEFAULT_DPI;
        public double ConvertedHeight { get { return _convertedHeight; } set { _convertedHeight = value; OnPropertyChanged(); } }

        private double _convertedGuideWidth = (LabelOption.DEFAULT_WIDTH * PrinterOption.DEFAULT_DPI) - (LabelOption.DEFAULT_MARGIN * PrinterOption.DEFAULT_DPI * 2);
        public double ConvertedGuideWidth { get { return _convertedGuideWidth; } set { _convertedGuideWidth = value; OnPropertyChanged(); } }

        private double _convertedGuideHeight = (LabelOption.DEFAULT_HEIGHT * PrinterOption.DEFAULT_DPI) - (LabelOption.DEFAULT_MARGIN * PrinterOption.DEFAULT_DPI * 2);
        public double ConvertedGuideHeight { get { return _convertedGuideHeight; } set { _convertedGuideHeight = value; OnPropertyChanged(); } }

        private double ValidWidth(double value)
        {
            if (value < LabelOption.MIN_WIDTH)
                return LabelOption.MIN_WIDTH;

            switch (SelectedDpi)
            {
                case PrinterOption.DPI_152:
                    if (value > LabelOption.MAX_WIDTH_152)
                        return LabelOption.MAX_WIDTH_152;
                    break;
                case PrinterOption.DPI_203:
                    if (value > LabelOption.MAX_WIDTH_203)
                        return LabelOption.MAX_WIDTH_203;
                    break;
                case PrinterOption.DPI_300:
                    if (value > LabelOption.MAX_WIDTH_300)
                        return LabelOption.MAX_WIDTH_300;
                    break;
            }
            return value;
        }

        private double ValidHeight(double value)
        {
            if (value < LabelOption.MIN_HEIGHT)
                return LabelOption.MIN_HEIGHT;

            switch (SelectedDpi)
            {
                case PrinterOption.DPI_152:
                    if (value > LabelOption.MAX_HEIGHT_152)
                        return LabelOption.MAX_HEIGHT_152;
                    break;
                case PrinterOption.DPI_203:
                    if (value > LabelOption.MAX_HEIGHT_203)
                        return LabelOption.MAX_HEIGHT_203;
                    break;
                case PrinterOption.DPI_300:
                    if (value > LabelOption.MAX_HEIGHT_300)
                        return LabelOption.MAX_HEIGHT_300;
                    break;
            }
            return value;
        }

        private void ResetActualSize()
        {
            ConvertedWidth = Width * SelectedDpi;
            ConvertedHeight = Height * SelectedDpi;
            ConvertedGuideWidth = ConvertedWidth - (Margin * SelectedDpi * 2);
            ConvertedGuideHeight = ConvertedHeight - (Margin * SelectedDpi * 2);
        }

        #endregion Vector Properties

        #region View Properties

        private int _margin = LabelOption.DEFAULT_MARGIN;
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

        private double _radius = LabelOption.DEFAULT_RADIUS;
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

        private double _viewSize = LabelOption.DEFAULT_VIEW_SIZE;
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

        private PrinterOption.PrinterType _selectedPrinter = PrinterOption.DEFAULT_PRINTER_TYPE;
        public PrinterOption.PrinterType SelectedPrinter { get { return _selectedPrinter; } set { _selectedPrinter = value; OnPropertyChanged(); } }

        private double _selectedDpi = PrinterOption.DEFAULT_DPI;
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

        private int _serialNumberStartIndex = 1;
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
