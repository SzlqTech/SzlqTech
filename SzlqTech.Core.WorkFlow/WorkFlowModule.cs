using Prism.Ioc;
using Prism.Modularity;
using SzlqTech.Core.WorkFlow.ViewModels;
using SzlqTech.Core.WorkFlow.Views;


namespace SzlqTech.Core.WorkFlow
{
    public class WorkFlowModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry service)
        {
            service.RegisterForNavigation<InnoLightTraceView, InnoLightTraceViewModel>();
            service.RegisterForNavigation<InnoLightDataRecord, InnoLightDataRecordViewModel>();
        }
    }
}
