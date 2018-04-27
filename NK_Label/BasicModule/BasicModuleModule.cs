using Microsoft.Practices.Unity;
using BasicModule.Views;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace BasicModule
{
    public class BasicModuleModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public BasicModuleModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<LabelView>(); //추후 새 윈도우 추가
        }
    }
}
