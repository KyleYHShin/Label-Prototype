﻿using BasicModule.Common;

namespace BasicModule.Models.Rule.Content
{
    public class RuleManualList : NotifyPropertyChanged, IRuleObject
    {
        #region Properties

        private ObservableDictionary<string, string> _contentList = new ObservableDictionary<string, string>();
        public ObservableDictionary<string, string> ContentList { get { return _contentList; } set { _contentList = value; OnPropertyChanged(); } }

        private string _selectedContent;
        public string SelectedContent { get { return _selectedContent; } set { _selectedContent = value; OnPropertyChanged(); } }

        #endregion Properties

        public bool AddList(string key, string value)
        {
            if (ContentList == null)
                ContentList = new ObservableDictionary<string, string>();

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
                OnPropertyChanged();
                return true;
            }
            return false;
        }

        #region Rule Common

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

        #endregion Rule Common
    }
}
