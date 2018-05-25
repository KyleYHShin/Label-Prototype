using BasicModule.Models.Rule;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZXing;

namespace BasicModule.Files
{
    public class FileData_1
    {
        public int FileVersion { get; set; }

        public LabelFile_1 Label { get; set; }
        public List<TextFile_1> TextList { get; set; }
        public List<BarcodeFile_1> BarcodeList { get; set; }

        public List<RuleSequFile_1> RuleSequentialNumList { get; set; }
        public List<RuleTimeFile_1> RuleTimeList { get; set; }
        public List<RuleManuFile_1> RuleManualList { get; set; }
        public List<RuleInputFile_1> RuleInputList { get; set; }
        public List<RuleInputCombineFile_1> RuleInputCombineList { get; set; }
    }

    public class LabelFile_1
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public int Margin { get; set; }
        public double Radius { get; set; }

        public Models.Option.PrinterOption.PrinterType SelectedPrinter { get; set; }
        public double SelectedDpi { get; set; }
        public ushort OffsetX { get; set; }
        public ushort OffsetY { get; set; }
        public int NumberOfCopies { get; set; }

        public int RepeatOfInputs { get; set; }
        public bool EnableSequentialInputs { get; set; }
        public int SerialNumberStartIndex { get; set; }
        public int SerialNumberLength { get; set; }
        public string LastSerialNumber { get; set; }
    }

    public class TextFile_1
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

    public class BarcodeFile_1
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


    public class RuleSequFile_1
    {
        public RuleRegulation.RuleFormat Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RSContent Contents { get; set; }

        public class RSContent
        {
            public byte NumLength { get; set; }
            public ulong MaxNum { get; set; }
            public ulong MinNum { get; set; }
            public ulong CurrNum { get; set; }
            public uint Increment { get; set; }
            public bool OnZeroFiller { get; set; }
        }
    }

    public class RuleTimeFile_1
    {
        public RuleRegulation.RuleFormat Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RTContent Contents { get; set; }

        public class RTContent
        {
            public string Pattern { get; set; }
        }
    }

    public class RuleManuFile_1
    {
        public RuleRegulation.RuleFormat Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.Xml.Linq.XElement Contents { get; set; }
        public string SelectedContent { get; set; }
    }

    public class RuleInputFile_1
    {
        public RuleRegulation.RuleFormat Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RIContent Contents { get; set; }

        public class RIContent
        {
            public int Order { get; set; }
            public int StartIndex { get; set; }
            public int CharLength { get; set; }
            public string InputData { get; set; }
        }
    }

    public class RuleInputCombineFile_1
    {
        public RuleRegulation.RuleFormat Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RICContent Contents { get; set; }

        public class RICContent
        {
            public string Seperator { get; set; }
            public int StartIndex { get; set; }
            public int CharLength { get; set; }
            public ObservableCollection<string> InputList { get; set; }
        }
    }
}