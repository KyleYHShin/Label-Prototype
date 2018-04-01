using BasicModule.Models;
using BasicModule.Models.Rule;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicModule.ViewModels
{
    public class PrintViewModel : BindableBase
    {

        private LabelObject _label = new LabelObject();
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

        private List<Rule> _ruleList;
        public List<Rule> RuleList
        {
            get { return _ruleList; }
            set { SetProperty(ref _ruleList, value); }
        }

        public PrintViewModel(LabelObject label, ObservableCollection<BasicObject> objectList, List<Rule> ruleList)
        {
            Label = label;
            ObjectList = objectList;
            foreach(var obj in objectList)
            {
                if (obj is TextObject)
                    (obj as TextObject).OriginText = (obj as TextObject).Text;
                else if (obj is BarcodeObject)
                    (obj as BarcodeObject).OriginText = (obj as BarcodeObject).Text;
            }
            RuleList = ruleList;
            //미사용 규칙 제거
        }

        public bool print()
        {
            List<RuleSequentialNum> rsnList = new List<RuleSequentialNum>();
            foreach(BasicObject obj in ObjectList)
            {
                string originText = "";
                if (obj is TextObject)
                    originText = (obj as TextObject).OriginText;
                else if (obj is BarcodeObject)
                    originText = (obj as BarcodeObject).OriginText;

                if (!string.IsNullOrEmpty(originText))
                {
                    string[] textBlocks = RuleRregulation.RuleNameSeperatorToList(originText);
                    foreach(var block in textBlocks)
                    {
                        if (RuleRregulation.RuleNameVerifier(block))
                        {
                            var rName = RuleRregulation.RuleNameExtractor(block);
                            foreach(Rule r in RuleList)
                            {
                                if (r.Name.Equals(rName)&& r.Format == RuleRregulation.RuleFormat.SEQUENTIAL_NO)
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

            if(rsnList.Count == 0)
            {
                //반복 횟수만큼 출력
                for(var i = 0; i<10; i++)
                {
                    ConvertRuleToText();
                    //print to window
                    //screenshot
                    //print
                }
            }else if (rsnList.Count == 1)
            {
                RuleSequentialNum rsn = rsnList[0];
                for (var i = rsn.CurrNum; i <= rsn.MaxNum; i += rsn.Increment)
                {
                    ConvertRuleToText();
                    //print to window
                    //screen shot
                    //print

                    //change rsn settings
                    rsn.CurrNum += rsn.Increment;
                }
            }
            else
            {
                // 오류! -> Dialog window
                return false;
            }


            ////이미지화
            //Bitmap img = null;
            //string[] oList = null;
            //bool ret = Utils.PrintService.PrintLabel(img, oList);

            return false;
        }

        private void ConvertRuleToText()
        {
            foreach (BasicObject obj in ObjectList)
            {
                string originText = "";
                if (obj is TextObject)
                    originText = (obj as TextObject).OriginText;
                else if (obj is BarcodeObject)
                    originText = (obj as BarcodeObject).OriginText;

                if (!string.IsNullOrEmpty(originText))
                {
                    string[] textBlocks = RuleRregulation.RuleNameSeperatorToList(originText);
                    for(int i = 0; i<textBlocks.Length; i++)
                    {
                        if (RuleRregulation.RuleNameVerifier(textBlocks[i]))
                        {
                            var rName = RuleRregulation.RuleNameExtractor(textBlocks[i]);
                            foreach (Rule r in RuleList)
                            {
                                if (r.Name.Equals(rName))
                                {
                                    textBlocks[i] = r.PrintValue();
                                    break;
                                }
                            }
                        }
                    }
                    if (obj is TextObject)
                        (obj as TextObject).Text = textBlocks.ToString();
                    else if (obj is BarcodeObject)
                        (obj as BarcodeObject).Text = textBlocks.ToString();
                }
            }
        }
    }
}
