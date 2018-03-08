using System;
using System.Collections.Generic;

namespace BasicModule.Files
{
    public class Data
    {
        public LabelFile Label { get; set; }
        public List<TextFile> TextList { get; set; }
        public List<BarcodeFile> BarcodeList { get; set; }
    }

    public class LabelFile
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public int SelectedPrinter { get; set; }
        public int SelectedDpi { get; set; }
        public double RadiusX { get; set; }
        public double RadiusY { get; set; }
    }

    public class TextFile
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }

        public System.Windows.Thickness Margin { get; set; }

        public string Text { get; set; }
        public int MaxLength { get; set; }
        public double FontSize { get; set; }
        public string TextAlignHorizen { get; set; }
        public string TextAlignVertical { get; set; }
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
        public string BarcodeType { get; set; }
    }
}
