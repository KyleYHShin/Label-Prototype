using NK_Label2.Models;
using NK_Label2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NK_Label2
{
    public class MainWindowViewModel
    {
        public ObservableCollection<TextModel> TextList { get; set; }
    }
}
