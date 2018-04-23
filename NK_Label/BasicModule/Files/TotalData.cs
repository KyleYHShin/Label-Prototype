using BasicModule.Models.Rule;
using System.Collections.Generic;
using ZXing;

namespace BasicModule.Files
{
    public class FileData
    {
        public string FileVersion { get; set; }

        public LabelFile Label { get; set; }
        public List<TextFile> TextList { get; set; }
        public List<BarcodeFile> BarcodeList { get; set; }

        public List<RuleSeq> RuleSequentialNumList { get; set; }
        public List<RuleTi> RuleTimeList { get; set; }
        public List<RuleManu> RuleManualList { get; set; }
    }

    public class LabelFile
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Margin { get; set; }

        public int SelectedPrinter { get; set; }
        public double SelectedDpi { get; set; }
        public double Radius { get; set; }
    }

    public class TextFile
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }

        public string Text { get; set; }
        public int MaxLength { get; set; }

        public double FontSize { get; set; }
        public string FontFamily { get; set; }
        public string FontStyle { get; set; }
        public string FontWeight { get; set; }
        public string TextAlignment { get; set; }
    }

    public class BarcodeFile
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }

        public string Text { get; set; }
        public int MaxLength { get; set; }
        public BarcodeFormat BarcodeType { get; set; }
    }


    public class RuleSeq
    {
        public RuleRregulation.RuleFormat Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RSContent Contents { get; set; }

        public class RSContent
        {
            public int NumLength { get; set; }
            public ulong MinNum { get; set; }
            public ulong MaxNum { get; set; }
            public ulong CurrNum { get; set; }
            public ulong Increment { get; set; }
        }
    }

    public class RuleTi
    {
        public RuleRregulation.RuleFormat Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RTContent Contents { get; set; }

        public class RTContent
        {
            public string Pattern { get; set; }
        }
    }

    public class RuleManu
    {
        public RuleRregulation.RuleFormat Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RMContent Contents { get; set; }

        public class RMContent
        {
            public List<KeyValuePair<string, string>> ContentList { get; set; }
            //public IList<string> KeyList { get; set; }
            //public IList<string> ValueList { get; set; }
            public string SelectedContent { get; set; }
        }
    }

}