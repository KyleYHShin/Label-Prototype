using System;
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
            // Initialize SystemLanguage by singleton pattern (ref LicenseController)
            // Initialize Theme by singleton pattern (ref LicenseController)
            string hardLockLoginErrMsg = Namkang.License.Controller.Login();

            if (!string.IsNullOrEmpty(hardLockLoginErrMsg))
            {
                BasicModule.Utils.DialogService.ShowSimpleTextDialog("Warning", hardLockLoginErrMsg);
                Shutdown();
            }
            else if(DateTime.Compare(Namkang.License.Controller.ProgramLicense.ServiceExpirationDate, NK_Label.Utils.SystemInfo.ReleaseDate) < 0)
            {
                BasicModule.Utils.DialogService.ShowSimpleTextDialog("Warning", "라이선스 기간이 만료되었습니다.");
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
