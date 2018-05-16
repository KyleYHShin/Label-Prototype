using NK_Label.License;
using System.Windows;

namespace NK_Label3
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string hardLockLoginErrMsg = LicenseController.LoginKey();
            if (!string.IsNullOrEmpty(hardLockLoginErrMsg))
            {
                BasicModule.Utils.DialogService.ShowSimpleTextDialog("Warning", hardLockLoginErrMsg);
                Shutdown();
            }
            else
            {
                base.OnStartup(e);
                var bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            
        }
    }
}
