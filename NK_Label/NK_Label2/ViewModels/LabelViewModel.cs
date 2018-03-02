using NK_Label2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NK_Label2.ViewModels
{
    public class LabelViewModel : INotifyPropertyChanged
    {
        private LabelModel _Label;
        private List<string> _ComponentList;

        public LabelModel Label
        {
            get { return _Label; }
            set
            {
                _Label = value;
                OnPropertyChanged("Label");
            }
        }

        public List<string> ComponentList
        {
            get { return _ComponentList; }
            set
            {
                _ComponentList = value;
                OnPropertyChanged("ComponentList");
            }
        }
        public string AddComponent
        {
            set
            {
                _ComponentList.Add(value);
                OnPropertyChanged("ComponentList");
            }
        }
        public string RemoveComponent
        {
            set
            {
                _ComponentList.Remove(value);
                OnPropertyChanged("ComponentList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
