using BasicModule.Models;
using Prism.Mvvm;

namespace BasicModule.ViewModels
{
    public class OptionBarcodeViewModel : BindableBase
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
    }
}
