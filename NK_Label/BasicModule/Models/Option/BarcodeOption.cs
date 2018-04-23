using BasicModule.Models.Common;
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
    }
}