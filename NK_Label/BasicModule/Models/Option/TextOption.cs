using System.Collections.Generic;

namespace BasicModule.Models.Option
{
    public static class TextOption
    {
        public static readonly Dictionary<string, string> FontStyleList = new Dictionary<string, string>
        {
            { "일반", "Normal" }
            , { "이탤릭체", "Italic" }
        };

        public static readonly Dictionary<string, string> FontWeightList = new Dictionary<string, string>
        {
            { "일반", "Normal" }
            , { "굵게", "Bold" }
        };

        public static readonly Dictionary<string, string> TextAlignmentList = new Dictionary<string, string>
        {
            { "왼쪽 정렬", "Left" }
            , { "가운데 정렬", "Center" }
            , { "오른쪽 정렬", "Right" }
        };

        public const double DEFAULT_WIDTH = 150; // pixel
        public const double DEFAULT_HEIGHT = 25;

        public const double MIN_WIDTH = 50;
        public const double MIN_HEIGHT = 15;

        public const double CONV_ADD_WIDTH = 10;
        public const double CONV_ADD_HEIGHT = 6;

        public const string DEFAULT_TEXT = "Text Contents";
        public const int DEFAULT_MAX_LENGTH = 30;

        public const double DEFAULT_FONT_SIZE = 20;
        public const double MIN_FONT_SIZE = 9;

        public const string DEFAULT_FONT_FAMILY = "Arial";
        public const string DEFAULT_FONT_STYLE = "Normal";
        public const string DEFAULT_FONT_WEIGHT = "Normal";
        public const string DEFAULT_TEXT_ALIGN = "Left";

    }
}
