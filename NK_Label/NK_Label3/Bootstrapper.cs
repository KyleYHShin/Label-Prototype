using Microsoft.Practices.Unity;
using Prism.Unity;
using NK_Label.Views;
using System.Windows;

namespace NK_Label
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
