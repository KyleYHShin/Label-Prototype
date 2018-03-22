using System.Collections.Generic;
using ZXing;

namespace BasicModule.Utils
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

    public class Option
    {
        public string Name { get; set; }
        public BarcodeFormat Key { get; set; }

        public Option(string name, BarcodeFormat key)
        {
            this.Name = name;
            this.Key = key;
        }
    }
}
