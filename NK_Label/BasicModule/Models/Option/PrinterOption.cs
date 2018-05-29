using System.Collections.Generic;

namespace BasicModule.Models.Option
{
    public static class PrinterOption
    {

        public static PrinterType DEFAULT_PRINTER_TYPE = PrinterType.ZEBRA;
        public enum PrinterType
        {
            ZEBRA = 1
        }

        public static readonly Dictionary<string, PrinterType> PrinterTypeList = new Dictionary<string, PrinterType>
        {
            { "Zebra", PrinterType.ZEBRA }
        };

        public const double DEFAULT_DPI = DPI_300;
        //Must be changed by testing
        public const double DPI_152 = 6;
        public const double DPI_203 = 8.013;
        public const double DPI_300 = 11.9;

        public static readonly Dictionary<string, double> DpiList = new Dictionary<string, double>
        {
            { "6dpmm(152 dpi)", DPI_152 }
            , { "8dpmm(203 dpi)", DPI_203 }
            , { "12dpmm(300 dpi)", DPI_300 }
        };

    }
}
