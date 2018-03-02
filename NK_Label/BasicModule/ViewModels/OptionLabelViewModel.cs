using BasicModule.Models;
using Prism.Mvvm;

namespace BasicModule.ViewModels
{
    public class OptionLabelViewModel : BindableBase
    {
        private LabelObject _labelObject;
        public LabelObject LabelObject{
            get { return _labelObject; }
            set { SetProperty(ref _labelObject, value); }
        }

        public OptionLabelViewModel(LabelObject lo)
        {
            LabelObject = lo;
        }
    }
}
