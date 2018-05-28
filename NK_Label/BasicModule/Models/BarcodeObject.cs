using BasicModule.Models.Common;
using BasicModule.Models.Option;
using BasicModule.Utils;
using System.Windows.Media.Imaging;
using ZXing;

namespace BasicModule.Models
{
    public class BarcodeObject : BasicObject, IPrintableObject
    {
        public LabelObject Label { get; set; }

        #region Vector Properties

        private double _width = BarcodeOption.DEFAULT_WIDTH;
        public new double Width
        {
            get { return _width; }
            set
            {
                if (value < BarcodeOption.MIN_WIDTH)
                    value = BarcodeOption.MIN_WIDTH;

                _width = getRound(value, 2);
                OnPropertyChanged();

                ChangeBarcodeImage();
            }
        }

        private double _height = BarcodeOption.DEFAULT_HEIGHT;
        public new double Height
        {
            get { return _height; }
            set
            {
                if (value < BarcodeOption.MIN_HEIGHT)
                    value = BarcodeOption.MIN_HEIGHT;

                _height = getRound(value, 2);
                OnPropertyChanged();

                ChangeBarcodeImage();
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
                BarcodeWriter = newWriter;
                OnPropertyChanged();

                ChangeBarcodeImage();
            }
        }

        private string _text = BarcodeOption.DEFAULT_TEXT;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();

                ChangeBarcodeImage();
            }
        }

        private void ChangeBarcodeImage()
        {
            try
            {
                if (BarcodeWriter != null)
                {
                    BarcodeWriter.Options.Width = (int)Width;
                    BarcodeWriter.Options.Height = (int)Height;

                    if (!string.IsNullOrEmpty(_text))
                        Barcode = BitmapConversion.BitmapToBitmapSource(BarcodeWriter.Write(_text));
                }
            }catch(System.Exception e)
            {
                Barcode = null;
            }
        }

        private int _maxLength = BarcodeOption.DEFAULT_MAX_LENGTH;
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
