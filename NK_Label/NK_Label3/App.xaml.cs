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
            var today = System.DateTime.Now;
            if (today.Year == 2018 && today.Month == 5 && today.Day > 1 && today.Day <= 31)
            {
                base.OnStartup(e);
                var bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            else
            {
                BasicModule.Utils.DialogService.ShowSimpleTextDialog("Warning", "테스트 기간이 만료되었습니다.");
                base.Shutdown();
            }
        }
    }
}
