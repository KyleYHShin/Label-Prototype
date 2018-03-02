using BasicModule.Models;
using Prism.Mvvm;

namespace BasicModule.ViewModels
{
    public class OptionTextViewModel : BindableBase
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
    }
}
