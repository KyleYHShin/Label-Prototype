using System.Collections.Generic;
using ZXing;

namespace BasicModule.Models.Option
{
    public static class BarcodeOption
    {
        public static IList<Option> BarcodeFormatList
        {
            get
            {
                return new List<Option>()
                {
                    new Option("DATA MATRIX", BarcodeFormat.DATA_MATRIX),
                    new Option("CODE 128", BarcodeFormat.CODE_128),
                    new Option("QR CODE", BarcodeFormat.QR_CODE)
                };
            }
        }
    }
}
