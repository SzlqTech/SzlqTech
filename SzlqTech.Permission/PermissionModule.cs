using Prism.Ioc;
using Prism.Modularity;
using SzlqTech.Permission.ViewModels;
using SzlqTech.Permission.Views;

namespace SzlqTech.Permission
{
    public class PermissionModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
           
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<UserManagerView, UserManagerViewModel>();
            containerRegistry.RegisterForNavigation<RoleManagerView, RoleManagerViewModel>();
            containerRegistry.RegisterForNavigation<MenuImportView, MenuImportViewModel>();
            containerRegistry.RegisterForNavigation<MenuAssignView, MenuAssignViewModel>();
        }
    }
}
