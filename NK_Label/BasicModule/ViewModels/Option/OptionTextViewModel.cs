using BasicModule.Common;
using BasicModule.Models;
using BasicModule.Models.Option;
using BasicModule.Views.Option;
using Prism.Commands;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.ViewModels.Option
{
    public class OptionTextViewModel : NotifyPropertyChanged, IOptionViewModel
    {
        public object FontStyleList { get => TextOption.FontStyleList; }
        public object FontWeightList { get => TextOption.FontWeightList; }
        public object TextAlignmentList { get => TextOption.TextAlignmentList; }

        private TextObject _textObject;
        public TextObject TextObject { get { return _textObject; } set { _textObject = value; OnPropertyChanged(); } }

        private bool IsCreateMode { get; set; }

        public OptionTextViewModel(TextObject to, bool isCreateMode)
        {
            TextObject = to;
            IsCreateMode = isCreateMode;
            ClickChangeObjectName = new DelegateCommand(ChangeObjectName);
        }

        public ICommand ClickChangeObjectName { get; private set; }
        public void ChangeObjectName()
        {
            ChangeNameWindow cnWin = new ChangeNameWindow()
            {
                Title = "'" + TextObject.Name + "' 의 이름 편집",
                Owner = Application.Current.MainWindow
            };
            cnWin.SetText(TextObject.Name);

            if (IsCreateMode)
                cnWin.ExistNameList = UsingLabelList.UsingObjectNameList;
            else
                cnWin.ExistNameList = UsingLabelList.UsingObjectNameListExceptSelectedObject;

            if (cnWin.ShowDialog() == true)
                TextObject.Name = cnWin.InputName.Text;
        }

        public bool isRight()
        {
            bool ret = !string.IsNullOrWhiteSpace(TextObject.Name)
                && TextObject.Width > 0
                && TextObject.Height > 0
                && TextObject.PosX >= 0
                && TextObject.PosY >= 0

                && !string.IsNullOrWhiteSpace(TextObject.Text)
                && TextObject.MaxLength > 0

                && TextObject.FontSize > 0
                && !string.IsNullOrEmpty(TextObject.FontFamily)
                && !string.IsNullOrEmpty(TextObject.FontStyle)
                && !string.IsNullOrEmpty(TextObject.FontWeight)
                && !string.IsNullOrEmpty(TextObject.TextAlignment);

            if (ret && UsingLabelList.UsingObjectNameList.Contains(TextObject.Name))
            {
                Utils.DialogService.ShowSimpleTextDialog("확인", "해당 텍스트의 이름(" + TextObject.Name + ")이 이미 사용중입니다.");
                ret = false;
            }

            if (ret)
                TextObject.IsChanged = true;
            return ret;
        }
    }
}
