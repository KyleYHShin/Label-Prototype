using BasicModule.Models;
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
                Data ret = new Data();

                LabelFile label = new LabelFile();
                label.Name = originData.Label.Name;
                label.Width = originData.Label.Width;
                label.Height = originData.Label.Height;
                label.RadiusX = originData.Label.RadiusX;
                label.RadiusY = originData.Label.RadiusY;
                ret.Label = label;

                ret.TextList = new List<TextFile>();
                ret.BarcodeList = new List<BarcodeFile>();
                foreach (var obj in originData.ObjectList)
                {
                    if (obj is TextObject)
                    {
                        var to = obj as TextObject;
                        TextFile tf = new TextFile();
                        tf.Name = to.Name;
                        tf.Width = to.Width;
                        tf.Height = to.Height;
                        tf.PosX = to.PosX;
                        tf.PosY = to.PosY;
                        tf.Margin = to.Margin;
                        tf.Text = to.Text;
                        tf.MaxLength = to.MaxLength;
                        tf.FontSize = to.FontSize;
                        tf.TextAlignHorizen = to.TextAlignHorizen;
                        tf.TextAlignVertical = to.TextAlignVertical;
                        ret.TextList.Add(tf);
                    }
                    else if (obj is BarcodeObject)
                    {
                        var bo = obj as BarcodeObject;
                        BarcodeFile bf = new BarcodeFile();
                        bf.Name = bo.Name;
                        bf.Width = bo.Width;
                        bf.Height = bo.Height;
                        bf.PosX = bo.PosX;
                        bf.PosY = bo.PosY;
                        bf.Text = bo.Text;
                        bf.MaxLength = bo.MaxLength;
                        bf.BarcodeType = bo.BarcodeType;
                        ret.BarcodeList.Add(bf);
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

        internal static string FileToObject(ref LabelViewModel labelData, Data fileData)
        {
            try
            {
                labelData.Label.Name = fileData.Label.Name;
                labelData.Label.Width = fileData.Label.Width;
                labelData.Label.Height = fileData.Label.Height;
                labelData.Label.RadiusX = fileData.Label.RadiusX;
                labelData.Label.RadiusY = fileData.Label.RadiusY;

                foreach (var to in fileData.TextList)
                {
                    labelData.ObjectList.Add(new TextObject()
                    {
                        Name = to.Name,
                        Width = to.Width,
                        Height = to.Height,
                        PosX = to.PosX,
                        PosY = to.PosY,
                        Margin = to.Margin,
                        Text = to.Text,
                        MaxLength = to.MaxLength,
                        FontSize = to.FontSize,
                        TextAlignHorizen = to.TextAlignHorizen,
                        TextAlignVertical = to.TextAlignVertical
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
                        Text = bo.Text,
                        MaxLength = bo.MaxLength,
                        BarcodeType = bo.BarcodeType
                    });
                }

                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion //ObjectToFile
    }
}
