using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.OneD;

namespace SmartAdorner.Demo
{
    public class IconViewModel : INotifyPropertyChanged
    {
        private bool m_isSelected;
        public bool IsSelected
        {
            get { return m_isSelected; }
            set { m_isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        private double m_X;
        public double X
        {
            get { return m_X; }
            set { m_X = value; OnPropertyChanged("X"); }
        }

        private double m_Y;
        public double Y
        {
            get { return m_Y; }
            set { m_Y = value; OnPropertyChanged("Y"); }
        }

        private IBarcodeWriter _barcodeWriter = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
        public IBarcodeWriter BarcodeWriter
        {
            get { return _barcodeWriter; }
            set
            {
                _barcodeWriter = value;
            }
        }

        public void changeBarcodeFormat(string format)
        {
            switch (format)
            {
                case "DATA_MATRIX":
                    _barcodeWriter.Format = BarcodeFormat.DATA_MATRIX;
                    _barcodeWriter.Options.Width = 90;
                    _barcodeWriter.Options.Height = 90;
                    _barcodeWriter.Options.PureBarcode = true;
                    Barcode = BitmapConversion.BitmapToBitmapSource(_barcodeWriter.Write(m_text));
                    break;
                case "CODE_128":
                    _barcodeWriter.Format = BarcodeFormat.CODE_128;
                    _barcodeWriter.Options.Width = 200;
                    _barcodeWriter.Options.Height = 70;
                    _barcodeWriter.Options.PureBarcode = true;
                    Bitmap bitmap128 = _barcodeWriter.Write(m_text);
                    Barcode = BitmapConversion.BitmapToBitmapSource(bitmap128);
                    break;
                case "QR_CODE":
                    _barcodeWriter.Format = BarcodeFormat.QR_CODE;
                    Barcode = BitmapConversion.BitmapToBitmapSource(new Bitmap(_barcodeWriter.Write(m_text)));
                    break;
            }
        }

        private String m_text;
        public String Text
        {
            get { return m_text; }
            set
            {
                m_text = value;
                OnPropertyChanged("Text");
                Barcode = BitmapConversion.BitmapToBitmapSource(new Bitmap(_barcodeWriter.Write(m_text)));
            }
        }

        private BitmapSource m_barcode;
        public BitmapSource Barcode
        {
            get { return m_barcode; }
            set { m_barcode = value; OnPropertyChanged("Barcode"); }
        }


        public static class BitmapConversion
        {
            public static BitmapSource BitmapToBitmapSource(Bitmap source)
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                              source.GetHbitmap(),
                              IntPtr.Zero,
                              Int32Rect.Empty,
                              BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private static Color BLACK = Color.FromArgb(0, 0, 0);
        private static Color WHITE = Color.FromArgb(255, 255, 255);

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


}
