using BasicModule.Models;
using Prism.Mvvm;

namespace BasicModule.ViewModels.Option
{
    public class OptionTextViewModel : BindableBase, IOptionViewModel
    {
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
            bool ret = !string.IsNullOrWhiteSpace(_textObject.Name)
                && _textObject.PosX >= 0
                && _textObject.PosY >= 0
                && !string.IsNullOrWhiteSpace(_textObject.Text)
                && _textObject.MaxLength > 0
                && _textObject.FontSize > 0;
            if (ret)
                _textObject.Changed = true;
            return ret;
        }
    }
}
