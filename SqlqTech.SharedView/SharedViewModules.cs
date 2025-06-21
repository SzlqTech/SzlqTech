using Prism.Ioc;
using Prism.Modularity;
using SqlqTech.SharedView.ViewModels;
using SqlqTech.SharedView.Views;


namespace SqlqTech.SharedView
{
    public class SharedViewModules : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry service)
        {
            service.RegisterForNavigation<MachineSettingView, MachineSettingViewModel>();       
            service.RegisterForNavigation<MachineDetailView, MachineDetailViewModel>();
            service.RegisterForNavigation<ScannerSettingView, ScannerSettingViewModel>();
            service.RegisterForNavigation<SysConfigView, SysConfigViewModel>();
            service.RegisterForNavigation<ProductView, ProductViewModel>();
            service.RegisterForNavigation<MachineDataCollectView, MachineDataCollectViewModel>();
        }
    }
}
