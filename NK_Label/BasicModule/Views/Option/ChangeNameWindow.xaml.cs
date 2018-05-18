using System.Collections.Generic;
using System.Windows;

namespace BasicModule.Views.Option
{
    /// <summary>
    /// ChangeNameWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChangeNameWindow : Window
    {
        public List<string> ExistNameList { get; set; }

        public ChangeNameWindow()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (!ExistNameList.Contains(TextInput.Text))
                DialogResult = true;
            else
            {
                Utils.DialogService.ShowSimpleTextDialog("Warning", "해당 이름이 이미 사용중입니다.");
                TextInput.Focus();
                TextInput.SelectionStart = TextInput.Text.Length;
            }
        }

        public void SetText(string text)
        {
            TextInput.Text = text;
            TextInput.Focus();
            TextInput.SelectionStart = TextInput.Text.Length;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
