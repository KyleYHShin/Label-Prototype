using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using NK_Label3.Views;
using System.Windows;
using BasicModule;

namespace NK_Label3
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

        protected override void ConfigureModuleCatalog()
        {
            var catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(BasicModuleModule));
        }
    }
}
