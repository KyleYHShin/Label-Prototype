using BasicModule.Common;
using BasicModule.Models.Common;

namespace BasicModule.Models.Rule
{
    public class RuleManualList : NotifyPropertyChanged, IRuleObject
    {
        private ObservableDictionary<string, string> _contentList;
        public ObservableDictionary<string, string> ContentList
        {
            get { return _contentList; }
            set
            {
                _contentList = value;
                OnPropertyChanged();
            }
        }

        private string _selectedContent;
        public string SelectedContent
        {
            get { return _selectedContent; }
            set
            {
                _selectedContent = value;
                OnPropertyChanged();
            }
        }

        public bool AddList(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrWhiteSpace(key)
                && !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                && ContentList != null && !ContentList.ContainsKey(key))
            {
                ContentList.Add(key, value);
                return true;
            }
            return false;
        }
        public bool UpdateList(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrWhiteSpace(key)
                && ContentList != null && ContentList.ContainsKey(key))
            {
                ContentList[key] = value;
                return true;
            }
            return false;
        }

        public bool RemoveList(string key)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrWhiteSpace(key)
                && ContentList != null && ContentList.ContainsKey(key))
            {
                ContentList.Remove(key);
                OnPropertyChanged("ContentList");
                return true;
            }
            return false;
        }

        public IRuleObject Clone
        {
            get
            {
                var obj = new RuleManualList();
                obj.ContentList = new ObservableDictionary<string, string>();
                foreach (var c in ContentList)
                {
                    obj.AddList(c.Key, c.Value);
                }
                obj.SelectedContent = SelectedContent;
                return obj;
            }
        }

        public string PrintValue => SelectedContent;
    }
}
