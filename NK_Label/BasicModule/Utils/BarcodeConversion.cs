using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace BasicModule.Utils
{
    public static class BarcodeConversion
    {
        public static void ChangeFormat(ref BarcodeWriter newWriter, string type)
        {
            switch (type)
            {
                case "DATA_MATRIX":
                    newWriter.Format = BarcodeFormat.DATA_MATRIX;
                    break;
                case "CODE_128":
                    newWriter.Format = BarcodeFormat.CODE_128;
                    break;
                case "QR_CODE":
                    newWriter.Format = BarcodeFormat.QR_CODE;
                    break;
            }
        }
    }
}
