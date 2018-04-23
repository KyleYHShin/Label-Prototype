using BasicModule.Models.Common;
using System.Collections.Generic;

namespace BasicModule.Models.Option
{
    public static class PrinterOption
    {
        public static IList<KeyValuePair> PrinterList
        {
            get
            {
                return new List<KeyValuePair>() { new KeyValuePair("Zebra", 1) };
            }
        }

        public static IList<KeyValuePair> DpiList
        {
            get
            {
                return new List<KeyValuePair>()
                {
                    new KeyValuePair("6dpmm(152 dpi)", 0.6),
                    new KeyValuePair("8dpmm(203 dpi)", 0.8),
                    new KeyValuePair("12dpmm(300 dpi)", 1.0),
                    //new BasicOption("24dpmm(600 dpi)", 2.4)
                };
            }
        }

        public static IList<KeyValuePair> LabelTypeList
        {
            get { return null; }
        }
    }
}
