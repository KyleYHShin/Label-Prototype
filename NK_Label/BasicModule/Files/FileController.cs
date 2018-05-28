using BasicModule.Utils;
using BasicModule.ViewModels;
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
            saveFileDialog.Filter = "NK Label-File | *.nkl";
            saveFileDialog.Title = "라벨 디자인 파일 저장";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                ret = SaveLabel_Xml(ref labelData, saveFileDialog.FileName);
            else
                ret = true;

            return ret;
        }

        private static bool SaveLabel_Xml(ref LabelViewModel labelData, string newPath = null)
        {
            string msg = "";
            bool ret = false;

            if (!String.IsNullOrEmpty(newPath))
                labelData.FilePath = newPath;

            switch (labelData.FileVersion)
            {
                case 1:
                    var obj_1 = Parser.ObjectToFile_1(labelData);
                    ret = XMLSerializer.Serialize(obj_1, labelData.FilePath, ref msg);
                    break;
            }

            if (!ret)
                MessageBox.Show("파일 저장 실패\n" + msg);

            return ret;
        }

        #endregion Save

        #region Open

        public static LabelViewModel OpenLabel(Prism.Regions.IRegionManager regionManager)
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
            openFileDialog.Filter = "NK Label-File | *.nkl";
            openFileDialog.Title = "라벨 디자인 파일 선택";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;

            return null;
        }

        private static bool OpenLabel_Xml(ref LabelViewModel labelData)
        {
            string msg = "";
            bool ret = false;

            int FileVersion = XMLSerializer.GetFileVersion(labelData.FilePath, ref msg);
            if (!String.IsNullOrEmpty(msg))
            {
                MessageBox.Show("파일 열기 실패\n" + msg);
                return false;
            }

            switch (FileVersion)
            {
                case 1:
                    var obj_1 = XMLSerializer.Deserializer(typeof(FileData_1), labelData.FilePath, ref msg) as FileData_1;
                    ret = Parser.FileToObject_1(ref labelData, obj_1);
                    break;
            }

            if (!String.IsNullOrEmpty(msg))
            {
                MessageBox.Show("파일 열기 실패\n" + msg);
                return false;
            }

            return ret;
        }

        #endregion Open

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