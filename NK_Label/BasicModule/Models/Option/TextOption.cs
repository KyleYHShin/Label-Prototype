using System.Collections.Generic;

namespace BasicModule.Models.Option
{
    public static class TextOption
    {
        public static readonly Dictionary<string, string> FontStyleList = new Dictionary<string, string>
        {
            { "Normal", "Normal" }
            , { "Italic", "Italic" }
        };

        public static readonly Dictionary<string, string> FontWeightList = new Dictionary<string, string>
        {
            { "Normal", "Normal" }
            , { "Bold", "Bold" }
        };

        public static readonly Dictionary<string, string> TextAlignmentList = new Dictionary<string, string>
        {
            { "Left", "Left" }
            , { "Center", "Center" }
            , { "Right", "Right" }
        };
    }
}
