using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Rule;
using BasicModule.ViewModels;

using System;
using System.Collections.Generic;

namespace BasicModule.Files
{
    internal static class Parser
    {
        #region ObjectToFile

        internal static Object ObjectToFile(LabelViewModel originData)
        {
            try
            {
                FileData ret = new FileData()
                {
                    FileVersion = "1.0.0",
                    TextList = new List<TextFile>(),
                    BarcodeList = new List<BarcodeFile>(),

                    RuleSequentialNumList = new List<RuleSeq>(),
                    RuleTimeList = new List<RuleTi>(),
                    RuleManualList = new List<RuleManu>()
                };

                ret.Label = GetLabelFile(originData.Label);

                foreach (var obj in originData.ObjectList)
                    GetObjectList(obj, ret.TextList, ret.BarcodeList);

                foreach (var rule in originData.RuleList)
                {
                    switch (rule.Format)
                    {
                        case RuleRregulation.RuleFormat.SEQUENTIAL_NUM:
                            GetRuleSequentialNumList(rule, ret.RuleSequentialNumList);
                            break;
                        case RuleRregulation.RuleFormat.TIME:
                            GetRuleTimeList(rule, ret.RuleTimeList);
                            break;
                        case RuleRregulation.RuleFormat.MANUAL_LIST:
                            GetRuleRuleManualList(rule, ret.RuleManualList);
                            break;
                    }
                }

                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private static LabelFile GetLabelFile(LabelObject lo)
        {
            return new LabelFile()
            {
                Name = lo.Name,
                Width = lo.Width,
                Height = lo.Height,
                Margin = lo.Margin,

                SelectedPrinter = lo.SelectedPrinter,
                SelectedDpi = lo.SelectedDpi,
                Radius = lo.Radius
            };
        }

        private static void GetObjectList(BasicObject obj, List<TextFile> TextList, List<BarcodeFile> BarcodeList)
        {
            if (obj is TextObject)
            {
                var to = obj as TextObject;
                TextList.Add(new TextFile()
                {
                    Name = to.Name,
                    Width = to.Width,
                    Height = to.Height,
                    PosX = to.PosX,
                    PosY = to.PosY,

                    Text = to.Text,
                    MaxLength = to.MaxLength,

                    FontSize = to.FontSize,
                    FontFamily = to.FontFamily,
                    FontStyle = to.FontStyle,
                    FontWeight = to.FontWeight,
                    TextAlignment = to.TextAlignment
                });
            }
            else if (obj is BarcodeObject)
            {
                var bo = obj as BarcodeObject;
                BarcodeList.Add(new BarcodeFile()
                {
                    Name = bo.Name,
                    Width = bo.Width,
                    Height = bo.Height,
                    PosX = bo.PosX,
                    PosY = bo.PosY,

                    MaxLength = bo.MaxLength,
                    BarcodeType = bo.BarcodeType,
                    Text = bo.Text
                });
            }
        }

        private static void GetRuleSequentialNumList(RuleMain rm, List<RuleSeq> RuleSequentialNumList)
        {
            var rsn = rm.Content as RuleSequentialNum;
            RuleSequentialNumList.Add(new RuleSeq()
            {
                Format = rm.Format,
                Name = rm.Name,
                Description = rm.Description,
                Contents = new RuleSeq.RSContent
                {
                    NumLength = rsn.NumLength,
                    MinNum = rsn.MinNum,
                    MaxNum = rsn.MaxNum,
                    CurrNum = rsn.CurrNum,
                    Increment = rsn.Increment
                }
            });
        }

        private static void GetRuleTimeList(RuleMain rm, List<RuleTi> RuleTimeList)
        {
            var rt = rm.Content as RuleTime;
            RuleTimeList.Add(new RuleTi()
            {
                Format = rm.Format,
                Name = rm.Name,
                Description = rm.Description,
                Contents = new RuleTi.RTContent
                {
                    Pattern = rt.Pattern
                }
            });
        }

        private static void GetRuleRuleManualList(RuleMain rm, List<RuleManu> RuleManualList)
        {            //RuleManualList.Add(new RuleManu()
            //{
            //    Format = rm.Format,
            //    Name = rm.Name,
            //    Description = rm.Description,
            //    Contents = new RuleManu.RMContent
            //    {
            //        ContentList = new KeyValuePair<string, string>(),
            //        SelectedContent = rml.SelectedContent
            //    }
            //});
            var rml = rm.Content as RuleManualList;
            var r = new RuleManu()
            {
                Format = rm.Format,
                Name = rm.Name,
                Description = rm.Description,
                Contents = new RuleManu.RMContent
                {
                    //ContentList = new Dictionary<string, string>(),
                    KeyList = new List<string>(),
                    ValueList = new List<string>(),
                    SelectedContent = rml.SelectedContent
                }
            };
            foreach (var pair in rml.ContentList)
            {
                //r.Contents.ContentList.Add(new KeyValuePair<string, string>(pair.Key, pair.Value));
                r.Contents.KeyList.Add(pair.Key);
                r.Contents.ValueList.Add(pair.Value);
            }
            RuleManualList.Add(r);
        }

        #endregion //ObjectToFile

        internal static string FileToObject(ref LabelViewModel labelData, FileData fileData)
        {
            try
            {
                labelData.Label = new LabelObject();
                labelData.Label.Name = fileData.Label.Name;
                labelData.Label.Width = fileData.Label.Width;
                labelData.Label.Height = fileData.Label.Height;
                labelData.Label.Margin = fileData.Label.Margin;
                labelData.Label.SelectedPrinter = fileData.Label.SelectedPrinter;
                labelData.Label.SelectedDpi = fileData.Label.SelectedDpi;
                labelData.Label.Radius = fileData.Label.Radius;

                foreach (var to in fileData.TextList)
                {
                    labelData.ObjectList.Add(new TextObject()
                    {
                        Name = to.Name,
                        Width = to.Width,
                        Height = to.Height,
                        PosX = to.PosX,
                        PosY = to.PosY,

                        Text = to.Text,
                        MaxLength = to.MaxLength,

                        FontSize = to.FontSize,
                        FontFamily = to.FontFamily,
                        FontStyle = to.FontStyle,
                        FontWeight = to.FontWeight,
                        TextAlignment = to.TextAlignment
                    });
                }

                foreach (var bo in fileData.BarcodeList)
                {
                    labelData.ObjectList.Add(new BarcodeObject()
                    {
                        Name = bo.Name,
                        Width = bo.Width,
                        Height = bo.Height,
                        PosX = bo.PosX,
                        PosY = bo.PosY,
                        MaxLength = bo.MaxLength,
                        BarcodeType = bo.BarcodeType,
                        Text = bo.Text
                    });
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}