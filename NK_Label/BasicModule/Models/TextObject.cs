using System.Windows;

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
                if (value != null && value.Length <= _maxLength)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxLength = 15;
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

        private double _fontSize = 14;
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

        private string _textAlignHorizen = "Left";
        public string TextAlignHorizen
        {
            get { return _textAlignHorizen; }
            set
            {
                if (value != null)
                {
                    _textAlignHorizen = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _textAlignVertical = "Top";
        public string TextAlignVertical
        {
            get { return _textAlignVertical; }
            set
            {
                if (value != null)
                {
                    _textAlignVertical = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion //Text Properties
        
        #region Vector Properties

        public double ActualPosX { get { return PosX - 10; } }
        public double ActualPosY { get { return PosY - 10; } }

        private Thickness _margin = new Thickness();
        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                OnPropertyChanged();
            }
        }

        #endregion //Vector Properties

        #region Decoration Properties

        //private string _borderColor = "Transparent";
        //public string BorderColor
        //{
        //    get { return _borderColor; }
        //    set { _borderColor = value; OnPropertyChanged(); }
        //}

        #endregion //Decoration Properties

        #region Control Properties

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                //if (value)
                //    BorderColor = "Red";
                //else
                //    BorderColor = "Transparent";
            }
        }

        #endregion //Control Properties

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
                obj.TextAlignHorizen = TextAlignHorizen;
                obj.TextAlignVertical = TextAlignVertical;
                obj.Margin = Margin;

                return obj;
            }
        }
    }
}
