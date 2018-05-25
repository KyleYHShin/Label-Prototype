using BasicModule.Models.Rule;
using BasicModule.Models.Rule.Content;
using BasicModule.ViewModels.Print;
using System;
using System.Windows;
using System.Windows.Input;

namespace BasicModule.Views.Print
{
    /// <summary>
    /// ReadyRuleInputListWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReadyRuleInputListWindow : Window
    {
        private PrintWindowViewModel thisDataContext;
        
        private int startIndex;
        private int length;

        public ReadyRuleInputListWindow()
        {
            InitializeComponent();
            MaxWidth = SystemParameters.PrimaryScreenWidth;
            MaxHeight = SystemParameters.PrimaryScreenHeight;
            SizeToContent = SizeToContent.WidthAndHeight;

            InputBox.Focus();
        }

        public void InitializeDataContext()
        {
            thisDataContext = DataContext as PrintWindowViewModel;

            if (thisDataContext.Label.EnableSequentialInputs)
            {
                if(thisDataContext.HasInputRule)
                    CheckSequentialBlock.Visibility = Visibility.Visible;
                else
                    CheckSequentialBlock.Visibility = Visibility.Collapsed;

                startIndex = thisDataContext.Label.SerialNumberStartIndex - 1;
                length = thisDataContext.Label.SerialNumberLength;
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Tab)
            {
                if (noEmptyInputs)
                    DialogResult = true;
            }
        }

        private bool noEmptyInputs
        {
            get
            {
                if (string.IsNullOrEmpty(InputBox.Text))
                    return false;

                if (thisDataContext.Label.EnableSequentialInputs)
                {
                    int newInputNum;
                    int lastInputNum;
                    if (Int32.TryParse(InputBox.Text.Substring(startIndex, length), out newInputNum)
                        && Int32.TryParse(LastSerialNumber.Text, out lastInputNum)
                        && lastInputNum+1 == newInputNum)
                    {
                       thisDataContext.Label.LastSerialNumber = InputBox.Text.Substring(startIndex, length);
                    }
                    else
                    {
                        InputBox.SelectAll();
                        return false;
                    }
                }

                for (int i = 0; i < InputList.Items.Count; i++)
                {
                    var ri = (InputList.Items[i] as RuleMain).Content as RuleInput;
                    if (string.IsNullOrEmpty(ri.InputData))
                    {
                        ri.InputData = InputBox.Text;
                        InputBox.Text = string.Empty;
                        InputBox.Focus();

                        if (i == InputList.Items.Count - 1)
                            return true;
                        else
                            return false;
                    }
                }
                return true;
            }
        }

        private void Click_Print(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
