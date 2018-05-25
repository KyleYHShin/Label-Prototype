using System.Collections.Generic;

namespace BasicModule.Models.Option
{
    public static class PrinterOption
    {

        public enum PrinterType
        {
            ZEBRA = 1
        }

        public static readonly Dictionary<string, PrinterType> PrinterList = new Dictionary<string, PrinterType>
        {
            { "Zebra", PrinterType.ZEBRA }
        };

        public const double DPI_LOW = 0.6;
        public const double DPI_MID = 0.8;
        public const double DPI_HIGH = 1.0;

        public static readonly Dictionary<string, double> DpiList = new Dictionary<string, double>
        {
            { "6dpmm(152 dpi)", DPI_LOW }
            , { "8dpmm(203 dpi)", DPI_MID }
            , { "12dpmm(300 dpi)", DPI_HIGH }
            //, { "24dpmm(600 dpi)", 2.4 }
        };
    }
}
