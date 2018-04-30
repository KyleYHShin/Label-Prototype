using BasicModule.Views.Common;
using System.Windows;

namespace BasicModule.Utils
{
    public static class DialogService
    {

        public static bool? ShowSimpleTextDialog(string title, string content)
        {
            var stView = new SimpleTextView();
            stView.TextContent.Text = content;

            var dlWin = new DialogWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = stView;
            dlWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            return dlWin.ShowDialog();
        }

        public static bool? ShowSimpleTextDialog(Window owner, string title, string content)
        {
            var stView = new SimpleTextView();
            stView.TextContent.Text = content;

            var dlWin = new DialogWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = stView;
            dlWin.Owner = owner;

            return dlWin.ShowDialog();
        }

        public static bool? ShowSimpleSelectDialog(string title, string text)
        {
            var stView = new SimpleTextView();
            stView.TextContent.Text = text;

            var dlWin = new DialogSelectWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = stView;
            dlWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            return dlWin.ShowDialog();
        }

        public static bool? ShowSimpleSelectDialog(Window owner, string title, string text)
        {
            var stView = new SimpleTextView();
            stView.TextContent.Text = text;

            var dlWin = new DialogSelectWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = stView;
            dlWin.Owner = owner;

            return dlWin.ShowDialog();
        }

        public static bool? ShowSelectDialog(Window owner, object contentView, string title)
        {
            var dlWin = new DialogSelectWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = contentView;
            dlWin.Owner = owner;

            return dlWin.ShowDialog();
        }

        public static bool? ShowContentDialog(object contentView, string title)
        {
            var dlWin = new DialogSelectWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = contentView;

            return dlWin.ShowDialog();
        }
    }
}
