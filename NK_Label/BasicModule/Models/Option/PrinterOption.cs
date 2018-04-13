using System.Collections.Generic;

namespace BasicModule.Models.Option
{
    public static class PrinterOption
    {
        public static IList<Option> PrinterList
        {
            get
            {
                return new List<Option>() { new Option("Zebra", 1) };
            }
        }

        public static IList<Option> DpiList
        {
            get
            {
                return new List<Option>()
                {
                    new Option("6dpmm(152 dpi)", 6),
                    new Option("8dpmm(203 dpi)", 8),
                    new Option("12dpmm(300 dpi)", 12),
                    new Option("24dpmm(600 dpi)", 24)
                };
            }
        }

        public static IList<Option> LabelTypeList
        {
            get { return null; }
        }
    }
}
