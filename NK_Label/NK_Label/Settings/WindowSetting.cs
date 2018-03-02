using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NK_Label.Settings
{
    public class WindowSetting : BindableBase
    {
        #region Attributes

        private string _TitleName = "NK-Label Prototype";

        //이후 Theme 으로 객체화 필요
        private string _TitleBackgroundColor1 = "#FF4E94DA";
        private string _TitleBackgroundColor2 = "Black";
        private int _TitleHeight = 25;
        private string _TitleNameColor = "White";
        private string _TitleNameSize = "14";

        private string _WindowBackground = "#FFCBCBCB";
        private string _FontFamily = "Malgun Gothic";
        private string _BorderBrush = "#FF5CAFFF";
        private int _BorderThickness = 1;

        #endregion // Attributes


        public WindowSetting()
        {
            //Original : Load languages from specific file
        }


        #region Functions

        public string TitleName
        {
            get { return _TitleName; }
            set { SetProperty(ref _TitleName, value); }
        }
        public string TitleBackgroundColor1
        {
            get { return _TitleBackgroundColor1; }
            set { SetProperty(ref _TitleBackgroundColor1, value); }
        }
        public string TitleBackgroundColor2
        {
            get { return _TitleBackgroundColor2; }
            set { SetProperty(ref _TitleBackgroundColor2, value); }
        }
        public int TitleHeight
        {
            get { return _TitleHeight; }
            set { SetProperty(ref _TitleHeight, value); }
        }
        public string TitleNameColor
        {
            get { return _TitleNameColor; }
            set { SetProperty(ref _TitleNameColor, value); }
        }
        public string TitleNameSize
        {
            get { return _TitleNameSize; }
            set { SetProperty(ref _TitleNameSize, value); }
        }

        public string WindowBackground
        {
            get { return _WindowBackground; }
            set { SetProperty(ref _WindowBackground, value); }
        }
        public string FontFamily
        {
            get { return _FontFamily; }
            set { SetProperty(ref _FontFamily, value); }
        }
        public string BorderBrush
        {
            get { return _BorderBrush; }
            set { SetProperty(ref _BorderBrush, value); }
        }
        public int BorderThickness
        {
            get { return _BorderThickness; }
            set { SetProperty(ref _BorderThickness, value); }
        }



        #endregion // Functions
    }
}
