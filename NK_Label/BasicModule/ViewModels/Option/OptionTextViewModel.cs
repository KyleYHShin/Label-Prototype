using BasicModule.Models;
using BasicModule.Models.Option;
using Prism.Mvvm;

namespace BasicModule.ViewModels.Option
{
    public class OptionTextViewModel : BindableBase, IOptionViewModel
    {
        public object FontStyleList { get => TextOption.FontStyleList; }
        public object FontWeightList { get => TextOption.FontWeightList; }
        public object TextAlignmentList { get => TextOption.TextAlignmentList; }

        private TextObject _textObject;
        public TextObject TextObject
        {
            get { return _textObject; }
            set { SetProperty(ref _textObject, value); }
        }

        public OptionTextViewModel(TextObject to)
        {
            TextObject = to;
        }

        public bool isRight()
        {
            bool ret = !string.IsNullOrWhiteSpace(TextObject.Name)
                && TextObject.Width > 0
                && TextObject.Height > 0
                && TextObject.PosX >= 0
                && TextObject.PosY >= 0

                && !string.IsNullOrWhiteSpace(TextObject.Text)
                && TextObject.MaxLength > 0

                && TextObject.FontSize > 0
                && !string.IsNullOrEmpty(TextObject.FontFamily)
                && !string.IsNullOrEmpty(TextObject.FontStyle)
                && !string.IsNullOrEmpty(TextObject.FontWeight)
                && !string.IsNullOrEmpty(TextObject.TextAlignment);

            if (ret)
                TextObject.Changed = true;
            return ret;
        }
    }
}
