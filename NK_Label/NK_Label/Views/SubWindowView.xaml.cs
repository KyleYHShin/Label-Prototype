using NK_Label.Models;
using NK_Label.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NK_Label.Views
{
    /// <summary>
    /// SubWindowView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SubWindowView : Window
    {
        public SubWindowView(/*FilePath*/)
        {
            ////test
            //LabelModel cm = new LabelModel();
            //List<ComponentModel> comList = new List<ComponentModel>();
            //comList.Add(new TextModel());

            //// Original : FileRead -> Create ObjectList -> make new LabelViewModel -> DataContext
            //this.DataContext = new LabelViewModel(cm, comList);

            InitializeComponent();
        }
    }
}
