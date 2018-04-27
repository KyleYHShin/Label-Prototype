using BasicModule.Common;
using Prism.Regions;
using System.Windows.Controls;

namespace BasicModule.Views.Rule
{
    /// <summary>
    /// RuleEditView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RuleListView : UserControl
    {        
        public RuleListView(IRegionManager regionManager)
        {
            InitializeComponent();

            if (regionManager != null)
            {
                RegionController.SetRegionManager(regionManager, this.RuleCommon, RegionNames.RuleCommon);
                RegionController.SetRegionManager(regionManager, this.RuleContent, RegionNames.RuleContent);
                RegionController.SetRegionManager(regionManager, this.RuleButton, RegionNames.RuleButton);
            }
        }
    }
}
