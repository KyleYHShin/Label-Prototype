using BasicModule.Common;
using BasicModule.Models;
using BasicModule.Models.Option;

namespace BasicModule.ViewModels.Option
{
    public class OptionBarcodeViewModel : NotifyPropertyChanged, IOptionViewModel
    {
        #region Properties
        
        public object BarcodeFormatList { get => BarcodeOption.BarcodeFormatList; }

        private BarcodeObject _barcodeObject;
        public BarcodeObject BarcodeObject { get { return _barcodeObject; } set { _barcodeObject = value; OnPropertyChanged(); } }

        #endregion Properties

        public OptionBarcodeViewModel(BarcodeObject bo)
        {
            BarcodeObject = bo;
        }

        public bool isRight()
        {
            bool ret = !string.IsNullOrWhiteSpace(BarcodeObject.Name)
                && BarcodeObject.Width > 0
                && BarcodeObject.Height > 0
                && BarcodeObject.PosX >= 0
                && BarcodeObject.PosY >= 0
                && BarcodeObject.BarcodeType > 0
                && !string.IsNullOrEmpty(BarcodeObject.Text)
                && BarcodeObject.MaxLength > 0;
            if (ret)
                BarcodeObject.IsChanged = true;

            return ret;
        }
    }
}
