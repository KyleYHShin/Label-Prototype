using BasicModule.Common;
using Prism.Regions;
using System.Windows;

namespace BasicModule.Views.Rule
{
    /// <summary>
    /// RuleManagerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RuleManagerWindow : Window
    {
        public RuleManagerWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;

            if (regionManager != null)
            {
                RegionController.SetRegionManager(regionManager, this.RuleCommon, RegionNames.RuleCommon);
                RegionController.SetRegionManager(regionManager, this.RuleContent, RegionNames.RuleContent);
                RegionController.SetRegionManager(regionManager, this.RuleButton, RegionNames.RuleButton);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
