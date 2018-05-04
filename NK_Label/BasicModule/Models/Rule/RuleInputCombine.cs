using BasicModule.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicModule.Models.Rule
{
    public class RuleInputCombine : NotifyPropertyChanged, IRuleObject
    {
        private ObservableCollection<string> _inputList;
        public ObservableCollection<string> InputList
        {
            get { return _inputList; }
            set
            {
                _inputList = value;
                OnPropertyChanged();
            }
        }

        public IRuleObject Clone => throw new NotImplementedException();

        public string PrintValue => throw new NotImplementedException();
    }
}
