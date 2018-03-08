using System.Collections.Generic;

namespace BasicModule.Models
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
                    new Option("8dpmm(203 dpi)", 1),
                    new Option("12dpmm(300 dpi)", 2),
                    new Option("24dpmm(600 dpi)", 3)
                };
            }
        }

        public static IList<Option> LabelTypeList
        {
            get { return null; }
        }
    }

    public class Option
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Option(string name, int id)
        {
            this.Name = name;
            this.Id = id;
        }
    }
}
