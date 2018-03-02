using BasicModule.Utils;
using System.Windows.Media.Imaging;
using ZXing;

namespace BasicModule.Models
{
    public class BarcodeObject : BasicObject
    {

        #region Barcode Properties

        private BarcodeWriter _barcodeWriter;
        public BarcodeWriter BarcodeWriter
        {
            get { return _barcodeWriter; }
            set
            {
                _barcodeWriter = value;
            }
        }

        private string _barcodeType;
        public string BarcodeType
        {
            get { return _barcodeType; }
            set
            {
                //if(value not in BarcodeFormat)
                //else
                _barcodeType = value;

                BarcodeWriter newWriter = new BarcodeWriter();
                BarcodeConversion.ChangeFormat(ref newWriter, value);
                newWriter.Options.PureBarcode = true;
                newWriter.Options.Margin = 0;
                newWriter.Options.Width = (int)Width;
                newWriter.Options.Height = (int)Height;

                BarcodeWriter = newWriter;
                Barcode = BitmapConversion.BitmapToBitmapSource(newWriter.Write(_text));

                OnPropertyChanged();
            }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                if (value != null && value.Length <= _maxLength)
                {
                    _text = value;
                    OnPropertyChanged();

                    if(BarcodeWriter != null)
                        Barcode = BitmapConversion.BitmapToBitmapSource(_barcodeWriter.Write(_text));
                }
            }
        }

        private int _maxLength = 15;
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
        public BitmapSource Barcode
        {
            get { return _barcode; }
            set { _barcode = value; OnPropertyChanged(); }
        }

        #endregion //Barcode Properties

        #region Vector Data

        private double _width = 100;
        public new double Width
        {
            get { return _width; }
            set
            {
                if (value > 0)
                {
                    _width = value;
                    OnPropertyChanged();

                    if (BarcodeWriter != null)
                    {
                        BarcodeWriter.Options.Width = (int)_width;
                        BarcodeWriter.Options.Height = (int)_height;
                        Barcode = BitmapConversion.BitmapToBitmapSource(BarcodeWriter.Write(_text));
                    }
                }
            }
        }

        private double _height = 30;
        public new double Height
        {
            get { return _height; }
            set
            {
                if (value > 0)
                {
                    _height = value;
                    OnPropertyChanged();

                    if (BarcodeWriter != null)
                    {
                        BarcodeWriter.Options.Width = (int)_width;
                        BarcodeWriter.Options.Height = (int)_height;
                        Barcode = BitmapConversion.BitmapToBitmapSource(BarcodeWriter.Write(_text));
                    }
                }
            }
        }

        #endregion //Vector Data

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

        #endregion //Decoration Properties

        #region Control Properties

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                //if (value)
                //    BorderColor = "Red";
                //else
                //    BorderColor = "Transparent";
            }
        }

        #endregion //Control Properties

    }
}
