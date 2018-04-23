using BasicModule.Models;
using BasicModule.Models.Option;
using Prism.Mvvm;

namespace BasicModule.ViewModels.Option
{
    public class OptionLabelViewModel : BindableBase, IOptionViewModel
    {
        #region Properties

        public object PrinterList { get => PrinterOption.PrinterList; } 
        public object DpiList { get => PrinterOption.DpiList; } 

        private LabelObject _labelObject;
        public LabelObject LabelObject
        {
            get { return _labelObject; }
            set { SetProperty(ref _labelObject, value); }
        }

        #endregion //Properties

        public OptionLabelViewModel(LabelObject lo)
        {
            LabelObject = lo;
        }

        public bool isRight()
        {
            bool ret = !string.IsNullOrWhiteSpace(_labelObject.Name)
                && _labelObject.Width > 0
                && _labelObject.Height > 0
                && _labelObject.SelectedPrinter > 0
                && _labelObject.SelectedDpi > 0
                && _labelObject.Margin >= 0
                && _labelObject.Radius >= 0;
            if (ret)
                _labelObject.Changed = true;

            return ret;
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
    }
}
