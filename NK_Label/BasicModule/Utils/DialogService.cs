using BasicModule.Views;
using System.Windows;

namespace BasicModule.Utils
{
    public interface IDialogService
    {
        bool? ShowDialog(object contentView, string title);
        bool? ShowDialog(Window owner, object contentView, string title);
        bool? ShowSimpleDialog(Window owner, string title, string content);
    }

    public class DialogService : IDialogService
    {
        public bool? ShowDialog(object contentView, string title)
        {
            var dlWin = new DialogWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = contentView;

            return dlWin.ShowDialog();
        }

        public bool? ShowDialog(Window owner, object contentView, string title)
        {
            var dlWin = new DialogWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = contentView;
            dlWin.Owner = owner;

            return dlWin.ShowDialog();
        }

        public bool? ShowSimpleDialog(Window owner, string title, string content)
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
    }
}
