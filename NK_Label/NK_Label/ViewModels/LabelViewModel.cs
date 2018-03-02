using NK_Label.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NK_Label.ViewModels
{
    public class LabelViewModel : BindableBase
    {
        #region Field

        private string _FilePath;
        private LabelModel _LabelData;
        private List<ComponentModel> _ComponentList;

        #endregion // Field


        public LabelViewModel(LabelModel labelData, List<ComponentModel> componentList)
        {
            _LabelData = labelData;
            _ComponentList = componentList;
        }
        public LabelViewModel(string filePath)
        {
            FilePath = filePath;
            //get data from file
            //set this field data
        }
        //Test Source
        public LabelViewModel()
        {
            _LabelData = new LabelModel();
            _ComponentList = new List<ComponentModel>();
            _ComponentList.Add(new TextModel());
        }


        #region Functions

        public string Title
        {
            get { return _LabelData.Name;  }
        }

        public bool SaveLabelData(string newFilePath = null)
        {
            if (newFilePath != null)
            {
                FilePath = newFilePath;
            }
            // _FilePath로 저장
            return true;
        }

        public string FilePath
        {
            get { return _FilePath; }
            set { SetProperty(ref _FilePath, value); }
        }
        public LabelModel LabelData
        {
            get { return _LabelData; }
            set { SetProperty(ref _LabelData, value); }
        }
        public List<ComponentModel> ComponentList
        {
            get { return _ComponentList; }
            set { SetProperty(ref _ComponentList, value); }
        }

        #endregion // Functions
    }
}
