using BasicModule.Models.Common;
using BasicModule.Models.Option;

namespace BasicModule.Models
{
    public class TextObject : BasicObject, IPrintableObject
    {
        public LabelObject Label { get; set; }

        #region Vector Properties

        private double _width = TextOption.DEFAULT_WIDTH;
        public new double Width
        {
            get { return _width; }
            set
            {
                if (value < TextOption.MIN_WIDTH)
                    value = TextOption.MIN_WIDTH;

                _width = getRound(value, 2);
                OnPropertyChanged();
                ConvertedWidth = _width + TextOption.CONV_ADD_WIDTH;
            }
        }

        private double _height = TextOption.DEFAULT_HEIGHT;
        public new double Height
        {
            get { return _height; }
            set
            {
                if (value < TextOption.MIN_HEIGHT)
                    value = TextOption.MIN_HEIGHT;

                _height = getRound(value, 2);
                OnPropertyChanged();
                ConvertedHeight = _height + TextOption.CONV_ADD_HEIGHT;
            }
        }

        private double _convertedWidth = TextOption.DEFAULT_WIDTH + TextOption.CONV_ADD_WIDTH;
        public double ConvertedWidth { get { return _convertedWidth; } set { _convertedWidth = value; OnPropertyChanged(); } }
        private double _convertedHeight = TextOption.DEFAULT_HEIGHT + TextOption.CONV_ADD_HEIGHT;
        public double ConvertedHeight { get { return _convertedHeight; } set { _convertedHeight = value; OnPropertyChanged(); } }

        #endregion Vector Properties

        #region Text Properties

        private string _text = TextOption.DEFAULT_TEXT;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        private int _maxLength = TextOption.DEFAULT_MAX_LENGTH;
        public int MaxLength
        {
            get { return _maxLength; }
            set
            {
                if (value > 0)
                {
                    _maxLength = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Text Properties

        #region Font Style Properties

        private double _fontSize = TextOption.DEFAULT_FONT_SIZE;
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (value < TextOption.MIN_FONT_SIZE)
                    value = TextOption.MIN_FONT_SIZE;

                _fontSize = value;
                OnPropertyChanged();
            }
        }

        private string _fontFamily = TextOption.DEFAULT_FONT_FAMILY;
        public string FontFamily { get { return _fontFamily; } set { _fontFamily = value; OnPropertyChanged(); } }

        private string _fontStyle = TextOption.DEFAULT_FONT_STYLE;
        public string FontStyle { get { return _fontStyle; } set { _fontStyle = value; OnPropertyChanged(); } }

        private string _fontWeight = TextOption.DEFAULT_FONT_WEIGHT;
        public string FontWeight { get { return _fontWeight; } set { _fontWeight = value; OnPropertyChanged(); } }

        private string _textAlignment = TextOption.DEFAULT_TEXT_ALIGN;
        public string TextAlignment { get { return _textAlignment; } set { _textAlignment = value; OnPropertyChanged(); } }

        #endregion Font Style Properties

        #region Print Properties

        public string OriginText { get; set; }

        public IPrintableObject Clone
        {
            get
            {
                var obj = new TextObject();
                obj.Name = Name;
                obj.Width = Width;
                obj.Height = Height;
                obj.PosX = PosX;
                obj.PosY = PosY;

                obj.Text = Text;
                obj.MaxLength = MaxLength;

                obj.FontSize = FontSize;
                obj.FontFamily = FontFamily;
                obj.FontStyle = FontStyle;
                obj.FontWeight = FontWeight;
                obj.TextAlignment = TextAlignment;

                return obj;
            }
        }

        #endregion Print Properties
    }
}
