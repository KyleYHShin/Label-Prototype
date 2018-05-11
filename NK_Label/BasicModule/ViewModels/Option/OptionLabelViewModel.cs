using BasicModule.Common;
using BasicModule.Models;
using BasicModule.Models.Option;

namespace BasicModule.ViewModels.Option
{
    public class OptionLabelViewModel : NotifyPropertyChanged, IOptionViewModel
    {
        #region Properties

        public object PrinterList { get => PrinterOption.PrinterList; }
        public object DpiList { get => PrinterOption.DpiList; }

        private LabelObject _labelObject;
        public LabelObject LabelObject { get { return _labelObject; } set { _labelObject = value; OnPropertyChanged(); } }

        #endregion Properties

        public OptionLabelViewModel(LabelObject lo)
        {
            LabelObject = lo;
        }

        public bool isRight()
        {
            bool ret = !string.IsNullOrWhiteSpace(LabelObject.Name)
                && LabelObject.Width > 0
                && LabelObject.Height > 0
                && LabelObject.Margin >= 0
                && LabelObject.Radius >= 0
                && LabelObject.SelectedPrinter > 0
                && LabelObject.SelectedDpi > 0
                && LabelObject.OffsetX >= 0
                && LabelObject.OffsetY >= 0;
            if (ret)
                LabelObject.IsChanged = true;

            return ret;
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
    }
}
