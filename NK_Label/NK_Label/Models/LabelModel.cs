using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NK_Label.Models
{
    public class LabelModel : ComponentModel
    {
        #region Attributes

        private int _Printer;
        private int _LabelType;
        private string _Background = "White";
        //Border Radius, etc

        #endregion // Attributes


        public LabelModel()
        {
            //default setting
            PositionX = 0;
            PositionY = 0;
            Width = 300;
            Height = 150;
        }


        #region Functions

        public int Printer
        {
            get { return _Printer; }
            set
            {
                SetProperty(ref _Printer, value);
            }
        }
        public int LabelType
        {
            get { return _LabelType; }
            set
            {
                //set this Attributes by NK Label Type
                SetProperty(ref _LabelType, value);
            }
        }
        public string Background
        {
            get { return _Background; }
            set { SetProperty(ref _Background, value); }
        }

        #endregion // Functions
    }
}
