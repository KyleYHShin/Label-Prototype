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

        // PrintWindow
        public static string PrintLabelView => "PrintLabelView";
        public static string PrintRuleSeq => "PrintRuleSeq";
        public static string PrintRuleManu => "PrintRuleManu";
        public static string PrintRuleTime => "PrintRuleTime";
        public static string PrintRuleInput => "PrintRuleInput";
        public static string PrintRuleInputCombine => "PrintRuleInputCombine";
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
