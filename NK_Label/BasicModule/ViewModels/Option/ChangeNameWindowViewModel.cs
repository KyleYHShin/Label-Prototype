using BasicModule.Common;
using BasicModule.Models.Common;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BasicModule.ViewModels.Option
{
    public class ChangeNameWindowViewModel : NotifyPropertyChanged
    {
        public List<string> ExistNameList { get; set; }
        public BasicObject Object { get; set; }

        public ChangeNameWindowViewModel()
        {

            ClickChangeObjectName = new DelegateCommand(ChangeObjectName);
        }

        public ICommand ClickChangeObjectName { get; private set; }
        public void ChangeObjectName()
        {
            //control dialogresult
            //this.getVi
            //if (!ExistNameList.Contains(Object.Name))
            //    DialogResult = true;
            //else
            //    Utils.DialogService.ShowSimpleTextDialog("Warning", "해당 이름이 이미 사용중입니다.");
        }
    }
}
