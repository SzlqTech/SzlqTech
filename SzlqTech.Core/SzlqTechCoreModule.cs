using Prism.Ioc;
using Prism.Modularity;
using SzlqTech.Core.Account.ViewModels;
using SzlqTech.Core.Account.Views;


namespace SzlqTech.Core
{
    public class SzlqTechCoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<InfoMessageView, InfoMessageViewModel>();
            containerRegistry.RegisterForNavigation<ErrorMessageView, ErrorMessageViewModel>();
        }
    }
}
