using BasicModule.Models.Common;
using BasicModule.Utils;
using System.Windows.Media.Imaging;
using ZXing;

namespace BasicModule.Models
{
    public class BarcodeObject : BasicObject, IPrintableObject
    {
        #region Vector Properties

        private double _width = 100;
        public new double Width
        {
            get { return _width; }
            set
            {
                if (value >= 45)
                {
                    _width = getRound(value, 2);
                    OnPropertyChanged();

                    if (BarcodeWriter != null)
                    {
                        BarcodeWriter.Options.Width = (int)_width;
                        BarcodeWriter.Options.Height = (int)_height;
                        if (BarcodeWriter != null && !string.IsNullOrEmpty(_text))
                            Barcode = BitmapConversion.BitmapToBitmapSource(BarcodeWriter.Write(_text));
                    }
                }
            }
        }

        private double _height = 100;
        public new double Height
        {
            get { return _height; }
            set
            {
                if (value >= 45)
                {
                    _height = getRound(value, 2);
                    OnPropertyChanged();

                    if (BarcodeWriter != null)
                    {
                        BarcodeWriter.Options.Width = (int)_width;
                        BarcodeWriter.Options.Height = (int)_height;
                        if (BarcodeWriter != null && !string.IsNullOrEmpty(_text))
                            Barcode = BitmapConversion.BitmapToBitmapSource(BarcodeWriter.Write(_text));
                    }
                }
            }
        }

        #endregion Vector Properties

        #region Barcode Properties

        private BarcodeWriter _barcodeWriter;
        public BarcodeWriter BarcodeWriter { get { return _barcodeWriter; } set { _barcodeWriter = value; } }

        private BarcodeFormat _barcodeType;
        public BarcodeFormat BarcodeType
        {
            get { return _barcodeType; }
            set
            {
                _barcodeType = value;

                BarcodeWriter newWriter = new BarcodeWriter();
                newWriter.Format = _barcodeType;
                newWriter.Options.PureBarcode = true;
                newWriter.Options.Margin = 0;
                newWriter.Options.Width = (int)Width;
                newWriter.Options.Height = (int)Height;

                BarcodeWriter = newWriter;
                if (BarcodeWriter != null && !string.IsNullOrEmpty(_text))
                    Barcode = BitmapConversion.BitmapToBitmapSource(newWriter.Write(_text));

                OnPropertyChanged();
            }
        }

        private string _text = string.Empty;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();

                if (BarcodeWriter != null && !string.IsNullOrEmpty(_text))
                    Barcode = BitmapConversion.BitmapToBitmapSource(_barcodeWriter.Write(_text));
            }
        }

        private int _maxLength = 50;
        public int MaxLength
        {
            get { return _maxLength; }
            set
            {
                if (value > 0)
                {
                    _maxLength = value;
                    OnPropertyChanged();
                }
            }
        }

        private BitmapSource _barcode;
        public BitmapSource Barcode { get { return _barcode; } set { _barcode = value; OnPropertyChanged(); } }

        #endregion Barcode Properties

        #region Decoration Properties

        //private string _borderBrush = "Transparent";
        //public string BorderBrush
        //{
        //    get { return _borderBrush; }
        //    set
        //    {
        //        _borderBrush = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _borderColor = "Transparent";
        //public string BorderColor
        //{
        //    get { return _borderColor; }
        //    set { _borderColor = value; OnPropertyChanged(); }
        //}

        #endregion Decoration Properties

        #region Control Properties

        private bool _isSelected;
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged(); } }

        #endregion Control Properties

        #region Print Properties

        public string OriginText { get; set; }

        public IPrintableObject Clone
        {
            get
            {
                var obj = new BarcodeObject();
                obj.Name = Name;
                obj.Width = Width;
                obj.Height = Height;
                obj.PosX = PosX;
                obj.PosY = PosY;

                obj.BarcodeWriter = BarcodeWriter;
                obj.BarcodeType = BarcodeType;
                obj.Text = Text;
                obj.MaxLength = MaxLength;
                obj.Barcode = Barcode;

                return obj;
            }
        }

        #endregion Print Properties
    }
}
