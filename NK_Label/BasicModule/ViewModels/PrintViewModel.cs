using BasicModule.Models;
using BasicModule.Models.Rule;
using BasicModule.Utils;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BasicModule.ViewModels
{
    public class PrintViewModel : BindableBase
    {
        #region Properties

        private LabelObject _label;
        public LabelObject Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        private ObservableCollection<BasicObject> _objectList;
        public ObservableCollection<BasicObject> ObjectList
        {
            get { return _objectList; }
            set { SetProperty(ref _objectList, value); }
        }

        private List<RuleMain> _ruleList;
        public List<RuleMain> RuleList
        {
            get { return _ruleList; }
            set { SetProperty(ref _ruleList, value); }
        }

        public int Repetition { get; set; } = 1;

        #endregion

        public PrintViewModel(LabelObject label, ObservableCollection<BasicObject> objectList, List<RuleMain> ruleList)
        {
            Label = label;
            ObjectList = new ObservableCollection<BasicObject>();
            foreach (var obj in objectList)
            {
                if (obj is IPrintableObject)
                {
                    IPrintableObject newObj = (obj as IPrintableObject).Clone;
                    newObj.OriginText = newObj.Text;
                    newObj.Text = "";
                    ObjectList.Add(newObj as BasicObject);
                }
            }
            RuleList = ruleList;
        }

        private List<RuleSequentialNum> getRuleSequentialNumList()
        {
            var rsnList = new List<RuleSequentialNum>();
            foreach (var obj in ObjectList)
            {
                if (obj is IPrintableObject)
                {
                    var pObj = obj as IPrintableObject;
                    if (!string.IsNullOrEmpty(pObj.OriginText))
                    {
                        string[] textBlocks = RuleRregulation.RuleNameSeperatorToList(pObj.OriginText);
                        foreach (var block in textBlocks)
                        {
                            if (RuleRregulation.RuleNameVerifier(block))
                            {
                                foreach (RuleMain r in RuleList)
                                {
                                    if (r.Name.Equals(block) && r.Format == RuleRregulation.RuleFormat.SEQUENTIAL_NUM)
                                    {
                                        var rsn = r.Content as RuleSequentialNum;
                                        if (!rsnList.Contains(rsn))
                                        {
                                            rsnList.Add(rsn);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rsnList;
        }

        public bool print()
        {
            List<RuleSequentialNum> rsnList = getRuleSequentialNumList();
            if (rsnList.Count == 0)
            {
                for (var i = 0; i < Repetition; i++)
                {
                    ConvertRuleToText();
                    //print to window
                    //screenshot
                    //print
                }
            }
            else if (rsnList.Count == 1)
            {
                RuleSequentialNum rsn = rsnList[0];
                for (var i = rsn.CurrNum; i <= rsn.MaxNum; i += rsn.Increment)
                {
                    rsn.CurrNum = i;
                    ConvertRuleToText();
                    //print to window
                    //screen shot
                    //print

                    ////이미지화
                    //Bitmap img = null;
                    //string[] oList = null;
                    //bool ret = Utils.PrintService.PrintLabel(img, oList);
                    var ps = new PrintService();
                    ps.Label = Label;
                    ps.ObjectList = ObjectList;
                    ps.PrintLabel();
                }
            }
            else
            {
                // 오류! -> Dialog window
                return false;
            }

            return false;
        }

        private void ConvertRuleToText()
        {
            foreach (var obj in ObjectList)
            {
                if (obj is IPrintableObject)
                {
                    var pObj = obj as IPrintableObject;
                    if (!string.IsNullOrEmpty(pObj.OriginText))
                    {
                        string[] textBlocks = RuleRregulation.RuleNameSeperatorToList(pObj.OriginText);
                        for (int i = 0; i < textBlocks.Length; i++)
                        {
                            if (RuleRregulation.RuleNameVerifier(textBlocks[i]))
                            {
                                var rName = RuleRregulation.RuleNameExtractor(textBlocks[i]);
                                foreach (RuleMain r in RuleList)
                                {
                                    if (r.Name.Equals(textBlocks[i]))
                                    {
                                        textBlocks[i] = r.PrintValue;
                                        break;
                                    }
                                }
                            }
                        }
                        pObj.Text = string.Join("", textBlocks);
                    }
                }
            }
        }
    }
}
