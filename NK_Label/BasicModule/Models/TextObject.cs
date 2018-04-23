using BasicModule.Models.Common;

namespace BasicModule.Models
{
    public class TextObject : BasicObject, IPrintableObject
    {
        #region Text Properties

        private string _text = "TextBox";
        public string Text
        {
            get { return _text; }
            set
            {
                if (value != null)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxLength = 20;
        public int MaxLength
        {
            get { return _maxLength; }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > int.MaxValue)
                    value = int.MaxValue;

                _maxLength = value;
                OnPropertyChanged();
            }
        }

        #endregion //Text Properties

        #region Vector Properties


        private double _width = 100;
        public new double Width
        {
            get { return _width; }
            set
            {
                if (value > 0)
                {
                    _width = getRound(value, 2);
                    OnPropertyChanged();
                    ConvertedWidth = _width + 10;
                }
            }
        }

        private double _height = 35;
        public new double Height
        {
            get { return _height; }
            set
            {
                if (value > 0)
                {
                    _height = getRound(value, 2);
                    OnPropertyChanged();
                    ConvertedHeight = _height + 6;
                }
            }
        }

        private double _convertedWidth = 110;
        public double ConvertedWidth { get { return _convertedWidth; } set { _convertedWidth = value; OnPropertyChanged(); } }
        private double _convertedHeight = 41;
        public double ConvertedHeight { get { return _convertedHeight; } set { _convertedHeight = value; OnPropertyChanged(); } }

        #endregion Vector Properties

        #region Font Style Properties

        private double _fontSize = 25;
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (value >= 0)
                {
                    _fontSize = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _fontFamily = "Arial";
        public string FontFamily { get { return _fontFamily; } set { _fontFamily = value; OnPropertyChanged(); } }

        private string _fontStyle = "Normal";
        public string FontStyle { get { return _fontStyle; } set { _fontStyle = value; OnPropertyChanged(); } }

        private string _fontWeight = "Normal";
        public string FontWeight { get { return _fontWeight; } set { _fontWeight = value; OnPropertyChanged(); } }

        private string _textAlignment = "Left";
        public string TextAlignment { get { return _textAlignment; } set { _textAlignment = value; OnPropertyChanged(); } }

        #endregion Font Style Properties

        #region Control Properties

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                Visibility = IsSelected ? "Hidden" : "Visible";
                OnPropertyChanged();
            }
        }

        private string _visibility = "Visible";
        public string Visibility { get { return _visibility; } set { _visibility = value; OnPropertyChanged(); } }

        #endregion Control Properties

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
    }
}
