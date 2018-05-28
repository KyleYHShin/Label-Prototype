using BasicModule.Common;
using BasicModule.Models;
using BasicModule.Models.Option;
using BasicModule.Views.Option;
using Prism.Commands;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.ViewModels.Option
{
    public class OptionBarcodeViewModel : NotifyPropertyChanged, IOptionViewModel
    {
        #region Properties
        
        public object BarcodeFormatList { get => BarcodeOption.BarcodeFormatList; }

        private BarcodeObject _barcodeObject;
        public BarcodeObject BarcodeObject { get { return _barcodeObject; } set { _barcodeObject = value; OnPropertyChanged(); } }

        private bool IsCreateMode { get; set; }

        #endregion Properties

        public OptionBarcodeViewModel(BarcodeObject bo, bool isCreateMode)
        {
            BarcodeObject = bo;
            IsCreateMode = isCreateMode;
            ClickChangeObjectName = new DelegateCommand(ChangeObjectName);
        }

        public ICommand ClickChangeObjectName { get; private set; }
        public void ChangeObjectName()
        {
            ChangeNameWindow cnWin = new ChangeNameWindow()
            {
                Title = "'" + BarcodeObject.Name + "' 의 이름 편집",
                Owner = Application.Current.MainWindow
            };
            cnWin.SetText(BarcodeObject.Name);

            if (IsCreateMode)
                cnWin.ExistNameList = UsingLabelList.UsingObjectNameList;
            else
                cnWin.ExistNameList = UsingLabelList.UsingObjectNameListExceptSelectedObject;

            if (cnWin.ShowDialog() == true)
                BarcodeObject.Name = cnWin.InputName.Text;
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

            if (ret && UsingLabelList.UsingObjectNameList.Contains(BarcodeObject.Name))
            {
                Utils.DialogService.ShowSimpleTextDialog("확인", "해당 바코드의 이름(" + BarcodeObject.Name + ")이 이미 사용중입니다.");
                ret = false;
            }

            if (ret)
                BarcodeObject.IsChanged = true;

            return ret;
        }
    }
}
