using System.Collections.Generic;

namespace BasicModule.Models.Option
{
    public static class PrinterOption
    {
        public static readonly Dictionary<string, int> PrinterList = new Dictionary<string, int>
        {
            { "Zebra", 1 }
        };

        public static readonly Dictionary<string, double> DpiList = new Dictionary<string, double>
        {
            { "6dpmm(152 dpi)", 0.6 }
            , { "8dpmm(203 dpi)", 0.8 }
            , { "12dpmm(300 dpi)", 1.0 }
            //, { "24dpmm(600 dpi)", 2.4 }
        };
    }
}
