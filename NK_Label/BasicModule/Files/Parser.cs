using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Rule;
using BasicModule.Utils;
using BasicModule.ViewModels;

using System;
using System.Collections.Generic;

namespace BasicModule.Files
{
    internal static class Parser
    {
        #region Version 1

        internal static Object ObjectToFile_1(LabelViewModel originData)
        {
            try
            {
                FileData_1 ret = new FileData_1()
                {
                    FileVersion = originData.FileVersion,
                    TextList = new List<TextFile_1>(),
                    BarcodeList = new List<BarcodeFile_1>(),

                    RuleSequentialNumList = new List<RuleSequFile_1>(),
                    RuleTimeList = new List<RuleTimeFile_1>(),
                    RuleManualList = new List<RuleManuFile_1>()
                };

                ret.Label = LabelToFile_1(originData.Label);

                foreach (var obj in originData.ObjectList)
                    ObjectListToFile_1(obj, ret.TextList, ret.BarcodeList);

                foreach (var rule in originData.RuleList)
                {
                    switch (rule.Format)
                    {
                        case RuleRegulation.RuleFormat.SEQUENTIAL_NUM:
                            RuleSequentialNumToFile_1(rule, ret.RuleSequentialNumList);
                            break;
                        case RuleRegulation.RuleFormat.TIME:
                            RuleTimeToFile_1(rule, ret.RuleTimeList);
                            break;
                        case RuleRegulation.RuleFormat.MANUAL_LIST:
                            RuleManualListToFile_1(rule, ret.RuleManualList);
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

        private static LabelFile_1 LabelToFile_1(LabelObject lo)
        {
            return new LabelFile_1()
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

        private static void ObjectListToFile_1(BasicObject obj, List<TextFile_1> TextList, List<BarcodeFile_1> BarcodeList)
        {
            if (obj is TextObject)
            {
                var to = obj as TextObject;
                TextList.Add(new TextFile_1()
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
                BarcodeList.Add(new BarcodeFile_1()
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

        private static void RuleSequentialNumToFile_1(RuleMain rm, List<RuleSequFile_1> RuleSequentialNumList)
        {
            var rsn = rm.Content as RuleSequentialNum;
            RuleSequentialNumList.Add(new RuleSequFile_1()
            {
                Format = rm.Format,
                Name = rm.Name,
                Description = rm.Description,
                Contents = new RuleSequFile_1.RSContent
                {
                    NumLength = rsn.NumLength,
                    MinNum = rsn.MinNum,
                    MaxNum = rsn.MaxNum,
                    CurrNum = rsn.CurrNum,
                    Increment = rsn.Increment
                }
            });
        }

        private static void RuleTimeToFile_1(RuleMain rm, List<RuleTimeFile_1> RuleTimeList)
        {
            var rt = rm.Content as RuleTime;
            RuleTimeList.Add(new RuleTimeFile_1()
            {
                Format = rm.Format,
                Name = rm.Name,
                Description = rm.Description,
                Contents = new RuleTimeFile_1.RTContent
                {
                    Pattern = rt.Pattern
                }
            });
        }

        private static void RuleManualListToFile_1(RuleMain rm, List<RuleManuFile_1> RuleManualList)
        {
            var rml = rm.Content as RuleManualList;
            RuleManualList.Add(new RuleManuFile_1()
            {
                Format = rm.Format,
                Name = rm.Name,
                Description = rm.Description,
                Contents = XMLSerializer.DictionaryToXml(rml.ContentList),
                SelectedContent = rml.SelectedContent
            });
        }
               
        internal static bool FileToObject_1(ref LabelViewModel labelData, FileData_1 fileData)
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

                foreach (var file in fileData.RuleSequentialNumList)
                {
                    var rsn = new RuleSequentialNum()
                    {
                        NumLength = file.Contents.NumLength,
                        MinNum = file.Contents.MinNum,
                        MaxNum = file.Contents.MaxNum,
                        CurrNum = file.Contents.CurrNum,
                        Increment = file.Contents.Increment,
                    };
                    labelData.RuleList.Add(new RuleMain()
                    {
                        Format = file.Format,
                        Name = file.Name,
                        Description = file.Description,
                        Content = rsn
                    });

                }
                foreach (var file in fileData.RuleTimeList)
                {
                    var rt = new RuleTime()
                    {
                        Pattern = file.Contents.Pattern
                    };
                    labelData.RuleList.Add(new RuleMain()
                    {
                        Format = file.Format,
                        Name = file.Name,
                        Description = file.Description,
                        Content = rt
                    });
                }
                foreach (var file in fileData.RuleManualList)
                {
                    var rml = new RuleManualList()
                    {
                        ContentList = XMLSerializer.XmlToDictionary(file.Contents),
                        SelectedContent = file.SelectedContent
                    };
                    labelData.RuleList.Add(new RuleMain()
                    {
                        Format = file.Format,
                        Name = file.Name,
                        Description = file.Description,
                        Content = rml
                    });
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion Version 1

    }
}