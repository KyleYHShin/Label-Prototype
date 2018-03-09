using BasicModule.Views;
using System.Windows;

namespace BasicModule.Utils
{
    public interface IDialogService
    {
        bool? ShowDialog(object contentView, string title);
        bool? ShowDialog(Window owner, object contentView, string title);
    }

    public class DialogService : IDialogService
    {
        public bool? ShowDialog(object contentView, string title)
        {
            var dlWin = new ChildWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = contentView;

            return dlWin.ShowDialog();
        }

        public bool? ShowDialog(Window owner, object contentView, string title)
        {
            var dlWin = new ChildWindow
            {
                Title = title
            };
            dlWin.PART_ContentControl.Content = contentView;
            dlWin.Owner = owner;

            return dlWin.ShowDialog();
        }
    }
}
