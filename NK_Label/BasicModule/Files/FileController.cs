using BasicModule.ViewModels;
using Prism.Regions;
using System;
using System.IO;
using System.Windows.Forms;

namespace BasicModule.Files
{
    public static class FileController
    {
        #region Save

        public static void SaveLabel(ref LabelViewModel currLabelData, bool isSaveAs)
        {
            try
            {
                if (isSaveAs)
                    while (!SaveLabel_FileDialog(ref currLabelData)) { }
                else
                {
                    if (String.IsNullOrEmpty(currLabelData.FilePath) || !IsValidPath(currLabelData.FilePath))
                        while (!SaveLabel_FileDialog(ref currLabelData)) { }
                    else
                        while (!SaveLabel_Xml(ref currLabelData)) { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static bool SaveLabel_FileDialog(ref LabelViewModel labelData)
        {
            bool ret;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML-File | *.xml";
            //saveFileDialog.Filter = "NK Label-File | *.nkl";
            saveFileDialog.Title = "Save a Label File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                ret = SaveLabel_Xml(ref labelData, saveFileDialog.FileName);
            else
                ret = true;

            return ret;
        }

        private static bool SaveLabel_Xml(ref LabelViewModel labelData, string newPath = null)
        {
            string msg = "";
            var ret = false;
            var obj = Parser.ObjectToFile(labelData);

            if (String.IsNullOrEmpty(newPath))
                ret = XMLSerializer.Serialize(obj, labelData.FilePath, ref msg);
            else
            {
                ret = XMLSerializer.Serialize(obj, newPath, ref msg);
                labelData.FilePath = newPath;
            }

            if (!ret)
                MessageBox.Show("파일 저장 실패\n" + msg);

            return ret;
        }

        #endregion //Save

        #region Open
        public static LabelViewModel OpenLabel(IRegionManager regionManager)
        {
            string path = OpenLabel_FileDialog();
            if (!IsValidPath(path))
                return null;

            var newLabelViewModel = new LabelViewModel(regionManager) { FilePath = path };
            if (OpenLabel_Xml(ref newLabelViewModel))
            {
                newLabelViewModel.IsChanged = false;
                return newLabelViewModel;
            }

            return null;
        }

        private static string OpenLabel_FileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML-File | *.xml";
            //openFileDialog.Filter = "NK Label-File | *.nkl";
            openFileDialog.Title = "Select a Label File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;

            return null;
        }

        private static bool OpenLabel_Xml(ref LabelViewModel labelData)
        {
            string msg = "";
            var obj = XMLSerializer.Deserializer(typeof(FileData), labelData.FilePath, ref msg) as FileData;

            if (!String.IsNullOrEmpty(msg))
            {
                MessageBox.Show("파일 열기 실패\n" + msg);
                return false;
            }

            if (!String.IsNullOrEmpty(Parser.FileToObject(ref labelData, obj)))
                return false;

            return true;
        }

        #endregion //Open

        private static bool IsValidPath(string filePath)
        {
            try
            {
                Path.GetFullPath(filePath);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}