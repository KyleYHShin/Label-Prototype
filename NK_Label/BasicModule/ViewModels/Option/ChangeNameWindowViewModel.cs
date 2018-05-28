using BasicModule.Common;
using BasicModule.Models.Common;
using System.Collections.Generic;

namespace BasicModule.ViewModels.Option
{
    public class ChangeNameWindowViewModel : NotifyPropertyChanged
    {
        public List<string> ExistNameList { get; set; }
        public BasicObject Object { get; set; }

        public ChangeNameWindowViewModel()
        {
        }
    }
}
