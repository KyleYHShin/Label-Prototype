using BasicModule.Common;
using BasicModule.Models;
using BasicModule.Models.Option;
using BasicModule.Views.Option;
using Prism.Commands;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.ViewModels.Option
{
    public class OptionLabelViewModel : NotifyPropertyChanged, IOptionViewModel
    {
        #region Properties

        public object PrinterList { get => PrinterOption.PrinterList; }
        public object DpiList { get => PrinterOption.DpiList; }

        private LabelObject _labelObject;
        public LabelObject LabelObject { get { return _labelObject; } set { _labelObject = value; OnPropertyChanged(); } }

        private bool IsCreateMode { get; set; }

        #endregion Properties

        public OptionLabelViewModel(LabelObject lo, bool isCreateMode)
        {
            LabelObject = lo;
            IsCreateMode = isCreateMode;
            ClickChangeObjectName = new DelegateCommand(ChangeObjectName);
        }

        public ICommand ClickChangeObjectName { get; private set; }
        public void ChangeObjectName()
        {
            ChangeNameWindow cnWin = new ChangeNameWindow()
            {
                Title = "Change '" + LabelObject.Name + "'s Name",
                Owner = Application.Current.MainWindow
            };
            cnWin.SetText(LabelObject.Name);

            if (IsCreateMode)
                cnWin.ExistNameList = UsingLabelList.UsingLabelNameList;
            else
                cnWin.ExistNameList = UsingLabelList.UsingLabelNameListExceptSelectedLabel;

            if (cnWin.ShowDialog() == true)
                LabelObject.Name = cnWin.InputName.Text;
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

            if(ret && UsingLabelList.UsingLabelNameList.Contains(LabelObject.Name))
            {
                Utils.DialogService.ShowSimpleTextDialog("Warning", "해당 라벨의 이름(" + LabelObject.Name + ")이 이미 사용중입니다.");
                ret = false;
            }

            if (ret)
                LabelObject.IsChanged = true;

            return ret;
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
    }
}
