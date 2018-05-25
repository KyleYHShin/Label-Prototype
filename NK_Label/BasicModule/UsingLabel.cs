using BasicModule.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BasicModule
{
    public static class UsingLabelList
    {
        public static LabelViewModel SelectedLabelViewModel;
        public static ObservableCollection<LabelViewModel> LabelViewModelList;

        public static List<string> UsingLabelNameList
        {
            get
            {
                var list = new List<string>();

                foreach (var LVM in LabelViewModelList)
                    list.Add(LVM.Label.Name);

                return list;
            }
        }

        public static List<string> UsingLabelNameListExceptSelectedLabel
        {
            get
            {
                var list = new List<string>();

                foreach (var LVM in LabelViewModelList)
                    list.Add(LVM.Label.Name);

                return list;
            }
        }

        public static List<string> UsingObjectNameList
        {
            get
            {
                var list = new List<string>();

                foreach (var obj in SelectedLabelViewModel.ObjectList)
                    list.Add(obj.Name);

                return list;
            }
        }

        public static List<string> UsingObjectNameListExceptSelectedObject
        {
            get
            {
                var list = new List<string>();

                foreach (var obj in SelectedLabelViewModel.ObjectList)
                    if (!obj.IsSelected)
                        list.Add(obj.Name);

                return list;
            }
        }


    }
}
