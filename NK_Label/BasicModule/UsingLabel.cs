using BasicModule.ViewModels;
using BasicModule.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BasicModule
{
    public static class UsingLabelList
    {
        public static LabelView SelectedLabelView;
        public static ObservableCollection<LabelView> LabelViewList;

        public static LabelViewModel SelectedLabelViewModel
        {
            get { return SelectedLabelView.DataContext as LabelViewModel; }
        }
        public static List<LabelViewModel> LabelViewModelList
        {
            get
            {
                var list = new List<LabelViewModel>();
                foreach (var view in LabelViewList)
                    list.Add(view.DataContext as LabelViewModel);

                return list;
            }
        }

        public static List<string> UsingLabelNameList
        {
            get
            {
                var list = new List<string>();

                foreach (var view in LabelViewList)
                    list.Add((view.DataContext as LabelViewModel).Label.Name);

                return list;
            }
        }

        public static List<string> UsingLabelNameListExceptSelectedLabel
        {
            get
            {
                var list = new List<string>();

                foreach (var view in LabelViewList)
                    if (view != SelectedLabelView)
                        list.Add((view.DataContext as LabelViewModel).Label.Name);

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
