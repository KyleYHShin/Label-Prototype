using BasicModule.Models;
using Prism.Mvvm;
using System;

namespace BasicModule.ViewModels
{
    public class OptionViewModel : BindableBase
    {
        #region Properties

        private Object _currentObject;
        public Object CurrentObject
        {
            get { return _currentObject; }
            set { SetProperty(ref _currentObject, value); }
        }

        private BasicObject currObj;
        public BasicObject CurrObj
        {
            get { return currObj; }
            set { SetProperty(ref currObj, value); }
        }


        #endregion //Properties

        public OptionViewModel(Object obj)
        {
            CurrentObject = obj;
            //if (obj is TextObject)
            //    CurrentObject = obj as TextObject;
            //else if (obj is BarcodeObject)
            //    CurrentObject = obj as BarcodeObject;
            //else
            //    CurrentObject = obj as LabelObject;
        }
    }
}
