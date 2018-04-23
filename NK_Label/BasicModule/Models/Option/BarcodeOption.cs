using BasicModule.Models.Common;
using System.Collections.Generic;
using ZXing;

namespace BasicModule.Models.Option
{
    public static class BarcodeOption
    {
        public static IList<KeyValuePair> BarcodeFormatList
        {
            get
            {
                return new List<KeyValuePair>()
                {
                    new KeyValuePair("DATA MATRIX", BarcodeFormat.DATA_MATRIX)
                    , new KeyValuePair("PDF 417", BarcodeFormat.PDF_417)
                    , new KeyValuePair("QR CODE", BarcodeFormat.QR_CODE)
                    , new KeyValuePair("CODE 128", BarcodeFormat.CODE_128)
                    , new KeyValuePair("AZTEC", BarcodeFormat.AZTEC)
                };
            }
        }
    }
}
