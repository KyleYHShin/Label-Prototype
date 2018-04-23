using BasicModule.Views.Common;
using System.Windows;

namespace BasicModule.Utils
{
    public interface IDialogService
    {
        bool? ShowSimpleTextDialog(Window owner, string title, string content);
        bool? ShowSimpleSelectDialog(Window owner, string title, string content);
        bool? ShowSelectDialog(Window owner, object contentView, string title);

        bool? ShowContentDialog(object contentView, string title);
    }

    public class DialogService : IDialogService
    {

        public bool? ShowSimpleTextDialog(Window owner, string title, string content)
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

        public bool? ShowSimpleSelectDialog(Window owner, string title, string content)
        {
            var stView = new SimpleTextView();
            stView.TextContent.Text = content;

            var dlWin = new DialogSelectWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = stView;
            dlWin.Owner = owner;

            return dlWin.ShowDialog();
        }

        public bool? ShowSelectDialog(Window owner, object contentView, string title)
        {
            var dlWin = new DialogSelectWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = contentView;
            dlWin.Owner = owner;

            return dlWin.ShowDialog();
        }


        public bool? ShowContentDialog(object contentView, string title)
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
