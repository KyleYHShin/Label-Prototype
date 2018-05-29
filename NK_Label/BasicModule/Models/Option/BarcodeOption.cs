using System.Collections.Generic;
using ZXing;

namespace BasicModule.Models.Option
{
    public static class BarcodeOption
    {
        public static readonly Dictionary<string, BarcodeFormat> BarcodeFormatList = new Dictionary<string, BarcodeFormat>
        {
            { "DATA MATRIX", BarcodeFormat.DATA_MATRIX }
            , { "PDF 417", BarcodeFormat.PDF_417 }
            , { "QR CODE", BarcodeFormat.QR_CODE }
            , { "CODE 128", BarcodeFormat.CODE_128 }
            , { "AZTEC", BarcodeFormat.AZTEC }
        };
        
        public const double DEFAULT_WIDTH = 100; // pixel
        public const double DEFAULT_HEIGHT = 100;

        public const double MIN_WIDTH = 40;
        public const double MIN_HEIGHT = 20;

        public const string DEFAULT_TEXT = "Barcode Contents";
        public const int DEFAULT_MAX_LENGTH = 30;
        public const string PARSING_ERR_CODE = "Error. Check input data.";

    }
}