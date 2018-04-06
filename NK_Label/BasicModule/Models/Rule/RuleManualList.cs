using System.Collections.Generic;

namespace BasicModule.Models.Rule
{
    public class RuleManualList : NotifyPropertyChanged, IRuleObject
    {
        private Dictionary<string, string> _contentList;
        public Dictionary<string, string> ContentList
        {
            get { return _contentList; }
            set
            {
                _contentList = value;
                OnPropertyChanged();
                //개별 추가(or 데이터 변경) 시 화면갱신 되는지 확인
                //안될경우 메서드 작성 후 onpropertychanged 호출
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
                && !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
            {
                ContentList.Add(key, value);
                return true;
            }
            return false;
        }

        public IRuleObject Clone
        {
            get
            {
                var obj = new RuleManualList();
                obj.ContentList = ContentList;
                return obj;
            }
        }

        public string PrintValue => SelectedContent;
    }
}
