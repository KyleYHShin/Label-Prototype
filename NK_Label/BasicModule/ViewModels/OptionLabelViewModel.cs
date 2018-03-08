using BasicModule.Models;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Data;

namespace BasicModule.ViewModels
{
    public class OptionLabelViewModel : BindableBase
    {
        //PrinterOption _PrinterOption = new PrinterOption();
        public CollectionView PrinterList { get { return new CollectionView(PrinterOption.PrinterList); } }
        public CollectionView DpiList { get { return new CollectionView(PrinterOption.DpiList); } }

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
