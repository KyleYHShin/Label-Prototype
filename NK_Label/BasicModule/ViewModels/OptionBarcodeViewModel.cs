using BasicModule.Models;
using Prism.Mvvm;

namespace BasicModule.ViewModels
{
    public class OptionBarcodeViewModel : BindableBase, IOptionViewModel
    {
        private BarcodeObject _barcodeObject;
        public BarcodeObject BarcodeObject
        {
            get { return _barcodeObject; }
            set { SetProperty(ref _barcodeObject, value); }
        }

        public OptionBarcodeViewModel(BarcodeObject bo)
        {
            BarcodeObject = bo;
        }

        public bool isRight()
        {
            return true;
        }
    }
}
