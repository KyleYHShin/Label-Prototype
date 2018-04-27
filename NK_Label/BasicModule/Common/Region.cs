using Prism.Regions;
using System.Windows;

namespace BasicModule.Common
{
    public static class RegionNames
    {
        // MainWindow
        public static string MenuRegion => "MenuRegion";
        public static string OptionRegion => "OptionRegion";

        // RuleListView
        public static string RuleCommon => "RuleCommon";
        public static string RuleContent => "RuleContent";
        public static string RuleButton => "RuleButton";

    }

    public static class RegionController
    {
        public static void SetRegionManager(IRegionManager regionManager, DependencyObject regionTarget, string regionName)
        {
            regionManager.Regions.Remove(regionName);
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, regionManager);
        }
    }
}
