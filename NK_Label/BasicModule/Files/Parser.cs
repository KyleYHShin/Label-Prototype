using BasicModule.Models;
using BasicModule.Models.Common;
using BasicModule.Models.Rule;
using BasicModule.Models.Rule.Content;
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
                    RuleManualList = new List<RuleManuFile_1>(),
                    RuleInputList = new List<RuleInputFile_1>(),
                    RuleInputCombineList = new List<RuleInputCombineFile_1>()
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
                        case RuleRegulation.RuleFormat.INPUT:
                            RuleInputListToFile_1(rule, ret.RuleInputList);
                            break;
                        case RuleRegulation.RuleFormat.INPUT_COMBINE:
                            RuleInputCombineFile_1(rule, ret.RuleInputCombineList);
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
                Radius = lo.Radius,

                SelectedPrinter = lo.SelectedPrinter,
                SelectedDpi = lo.SelectedDpi,
                OffsetX = lo.OffsetX,
                OffsetY = lo.OffsetY,
                NumberOfCopies = lo.NumberOfCopies,

                RepeatOfInputs = lo.RepeatOfInputs,
                EnableSequentialInputs = lo.EnableSequentialInputs,
                SerialNumberStartIndex = lo.SerialNumberStartIndex,
                SerialNumberLength = lo.SerialNumberLength,
                LastSerialNumber = lo.LastSerialNumber
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
                    MaxNum = rsn.MaxNum,
                    MinNum = rsn.MinNum,
                    CurrNum = rsn.CurrNum,
                    Increment = rsn.Increment,
                    OnZeroFiller = rsn.OnZeroFiller,
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

        private static void RuleInputListToFile_1(RuleMain rm, List<RuleInputFile_1> RuleInputList)
        {
            var ri = rm.Content as RuleInput;
            RuleInputList.Add(new RuleInputFile_1()
            {
                Format = rm.Format,
                Name = rm.Name,
                Description = rm.Description,
                Contents = new RuleInputFile_1.RIContent
                {
                    Order = ri.Order,
                    StartIndex = ri.StartIndex,
                    CharLength = ri.CharLength,
                    InputData = ri.InputData
                }
            });
        }

        private static void RuleInputCombineFile_1(RuleMain rm, List<RuleInputCombineFile_1> RuleInputCombineList)
        {
            var ric = rm.Content as RuleInputCombine;
            RuleInputCombineList.Add(new Files.RuleInputCombineFile_1()
            {
                Format = rm.Format,
                Name = rm.Name,
                Description = rm.Description,
                Contents = new RuleInputCombineFile_1.RICContent
                {
                    Seperator = ric.Seperator,
                    StartIndex = ric.StartIndex,
                    CharLength = ric.CharLength,
                    InputList = ric.InputList
                }
            });
        }


        internal static bool FileToObject_1(ref LabelViewModel labelData, FileData_1 fileData)
        {
            //try
            //{
            labelData.Label = new LabelObject();
            labelData.Label.Name = fileData.Label.Name;
            labelData.Label.Width = fileData.Label.Width;
            labelData.Label.Height = fileData.Label.Height;

            labelData.Label.Margin = fileData.Label.Margin;
            labelData.Label.Radius = fileData.Label.Radius;

            labelData.Label.SelectedPrinter = fileData.Label.SelectedPrinter;
            labelData.Label.SelectedDpi = fileData.Label.SelectedDpi;
            labelData.Label.OffsetX = fileData.Label.OffsetX;
            labelData.Label.OffsetY = fileData.Label.OffsetY;
            labelData.Label.NumberOfCopies = fileData.Label.NumberOfCopies;

            labelData.Label.RepeatOfInputs = fileData.Label.RepeatOfInputs;
            labelData.Label.EnableSequentialInputs = fileData.Label.EnableSequentialInputs;
            labelData.Label.SerialNumberStartIndex = fileData.Label.SerialNumberStartIndex;
            labelData.Label.SerialNumberLength = fileData.Label.SerialNumberLength;
            labelData.Label.LastSerialNumber = fileData.Label.LastSerialNumber;

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
                    MaxNum = file.Contents.MaxNum,
                    MinNum = file.Contents.MinNum,
                    CurrNum = file.Contents.CurrNum,
                    Increment = file.Contents.Increment,
                    OnZeroFiller = file.Contents.OnZeroFiller,
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
            foreach (var file in fileData.RuleInputList)
            {
                var ri = new RuleInput()
                {
                    Order = file.Contents.Order,
                    StartIndex = file.Contents.StartIndex,
                    CharLength = file.Contents.CharLength,
                    InputData = file.Contents.InputData
                };
                labelData.RuleList.Add(new RuleMain()
                {
                    Format = file.Format,
                    Name = file.Name,
                    Description = file.Description,
                    Content = ri
                });
            }
            foreach (var file in fileData.RuleInputCombineList)
            {
                var ric = new RuleInputCombine()
                {
                    Seperator = file.Contents.Seperator,
                    StartIndex = file.Contents.StartIndex,
                    CharLength = file.Contents.CharLength,
                    InputList = file.Contents.InputList
                };
                labelData.RuleList.Add(new RuleMain()
                {
                    Format = file.Format,
                    Name = file.Name,
                    Description = file.Description,
                    Content = ric
                });
            }
            return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return false;
            //}
        }

        #endregion Version 1

    }
}