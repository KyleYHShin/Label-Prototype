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
    }
}
