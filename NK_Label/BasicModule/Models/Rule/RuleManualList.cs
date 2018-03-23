using System.Collections.Generic;

namespace BasicModule.Models.Rule
{
    public class RuleManualList : INotifyProperty
    {
        private Dictionary<string, string> _contentList;
        public Dictionary<string, string> ContentList
        {
            get { return _contentList; }
            set
            {
                _contentList = value;
                OnPropertyChanged();
                //개별 추가 시 화면갱신 되는지 확인
                //안될경우 메서드 작성 후 onpropertychanged 호출
            }
        }
    }
}
