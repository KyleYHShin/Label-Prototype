using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NK_Label.Models
{
    public abstract class ComponentModel : BindableBase
    {
        #region Common Attributes

        private string _Name;
        private int _PositionX = 10;
        private int _PositionY = 10;
        private int _Width;
        private int _Height;
        private string _HorizontalAlignment = "Left";
        private string _VerticalAlignment = "Top";

        #endregion //Common Attributes


        #region Common Functions

        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        public string Margin
        {
            get { return _PositionX + "," + _PositionY + ",0,0"; }
            set
            {
                SetProperty(ref _PositionX, int.Parse(value.Split(',')[0]));
                SetProperty(ref _PositionY, int.Parse(value.Split(',')[1]));
            }
        }
        public int PositionX
        {
            get { return _PositionX; }
            set { SetProperty(ref _PositionX, value); }
        }
        public int PositionY
        {
            get { return _PositionY; }
            set { SetProperty(ref _PositionY, value); }
        }
        public int Width
        {
            get { return _Width; }
            set { SetProperty(ref _Width, value); }
        }
        public int Height
        {
            get { return _Height; }
            set { SetProperty(ref _Height, value); }
        }
        public string HorizontalAlignment
        {
            get { return _HorizontalAlignment; }
            set { SetProperty(ref _HorizontalAlignment, value); }
        }
        public string VerticalAlignment
        {
            get { return _VerticalAlignment; }
            set { SetProperty(ref _VerticalAlignment, value); }
        }

        #endregion //Common Functions
    }
}
