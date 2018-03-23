using BasicModule.Models;
using BasicModule.Models.Option;
using Prism.Mvvm;
using System.Windows.Data;

namespace BasicModule.ViewModels.Option
{
    public class OptionBarcodeViewModel : BindableBase, IOptionViewModel
    {
        #region Properties

        public CollectionView BarcodeFormatList { get { return new CollectionView(BarcodeOption.BarcodeFormatList); } }

        private BarcodeObject _barcodeObject;
        public BarcodeObject BarcodeObject
        {
            get { return _barcodeObject; }
            set { SetProperty(ref _barcodeObject, value); }
        }

        #endregion //Properties

        public OptionBarcodeViewModel(BarcodeObject bo)
        {
            BarcodeObject = bo;
        }

        public bool isRight()
        {
            bool ret = !string.IsNullOrWhiteSpace(_barcodeObject.Name)
                && _barcodeObject.Width > 0
                && _barcodeObject.Height > 0
                && _barcodeObject.PosX >= 0
                && _barcodeObject.PosY >= 0
                && _barcodeObject.BarcodeType > 0;
            if (ret)
                _barcodeObject.Changed = true;

            return ret;
        }
    }
}
