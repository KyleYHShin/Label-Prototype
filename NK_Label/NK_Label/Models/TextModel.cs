using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NK_Label.Models
{
    public class TextModel : ComponentModel
    {
        #region Attributes

        private string _Content = "Text";
        private string _Foreground = "Black";
        private int _FontSize = 15;

        #endregion // Attributes


        #region Functions

        public TextModel()
        {
            Width = 100;
            Height = 40;
        }

        public string Content
        {
            get { return _Content; }
            set
            {
                SetProperty(ref _Content, value);
            }
        }
        public string Foreground
        {
            get { return _Foreground; }
            set { SetProperty(ref _Foreground, value); }
        }
        public int FontSize
        {
            get { return _FontSize; }
            set { SetProperty(ref _FontSize, value); }
        }

        #endregion // Functions
    }
}
