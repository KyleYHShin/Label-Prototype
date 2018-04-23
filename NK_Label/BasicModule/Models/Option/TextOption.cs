using BasicModule.Models.Common;
using System.Collections.Generic;

namespace BasicModule.Models.Option
{
    public class TextOption
    {
        public IList<KeyValuePair> FontStyleList
        {
            get
            {
                return new List<KeyValuePair>()
                {
                    new KeyValuePair("Normal", "Normal")
                    , new KeyValuePair("Italic", "Italic")
                };
            }
        }

        public IList<KeyValuePair> FontWeightList
        {
            get
            {
                return new List<KeyValuePair>()
                {
                    new KeyValuePair("Normal", "Normal")
                    , new KeyValuePair("Bold", "Bold")
                };
            }
        }

        public IList<KeyValuePair> TextAlignmentList
        {
            get
            {
                return new List<KeyValuePair>()
                {
                    new KeyValuePair("Left", "Left")
                    , new KeyValuePair("Center", "Center")
                    , new KeyValuePair("Right", "Right")
                };
            }
        }
    }
}
