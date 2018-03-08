using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace BasicModule.ViewModels
{
    public class ChildWindowViewModel : BindableBase
    {
        #region Properties
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get
            {
                if(_currentViewModel is OptionLabelViewModel)
                    return _currentViewModel as OptionLabelViewModel;
                else
                    return null;
            }
            set { _currentViewModel = value; }
        }
        #endregion //Properties

        public ChildWindowViewModel()
        {
            BtnClickApply = new DelegateCommand(BtnApply);
            BtnClickCancle = new DelegateCommand(CloseWindow);
        }

        #region Button Event

        public ICommand BtnClickApply { get; private set; }
        public ICommand BtnClickCancle { get; private set; }

        private void BtnApply()
        {
            //if(check values) -> return LabelObject to MainWindowViewModel
        }
        private void CloseWindow()
        {
        }

        private DialogResult Close1(bool ret)
        {
            if (ret)
                return DialogResult.OK;
            else
                return DialogResult.Cancel;
        }

        #endregion //Button Event
    }
}
