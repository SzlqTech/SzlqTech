using AutoMapper;
using Prism.Ioc;
using SzlqTech.Core.Services.App;
using SzlqTech.Core.Services.Session;
using SzlqTech.IRepository;
using SzlqTech.IService;
using SzlqTech.Repository;
using SzlqTech.Service;
using SzlqTech.Services.Mapper;
using SzlqTech.Services.Sessions;

namespace SzlqTech.Extensions
{
    public static class ContainerExtensions
    {

        #region 注册数据库仓储服务
        public static void AddRepository(this IContainerRegistry container)
        {
            container.Register<ISysUserRepository, SysUserRepository>();
            container.Register<IMachineSettingRepository, MachineSettingRepository>();
            container.Register<IMachineDetailRepository, MachineDetailRepository>();
            container.Register<IScannerSettingRepository,ScannerSettingRepository>();
        }

        public static void AddDbService(this IContainerRegistry container)
        {
            container.Register<ISysUserService, SysUserServiceImpl>();
            container.Register<IMachineSettingService, MachineSettingServiceImpl>();
            container.Register<IMachineDetailService,MachineDetailServiceImpl>();
            container.Register<IScannerSettingService, ScannerSettingServiceImpl>();
        } 
        #endregion

        public static void AddService(this IContainerRegistry container)
        {
            container.RegisterSingleton<IAppStartService, MainStartService>();
            container.RegisterSingleton<IHostDialogService, DialogHostService>();
            //注册AutoMapper
            var config = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });
            container.RegisterInstance<IMapper>(config.CreateMapper());
            container.Register<NavigationService>();
        }
    }
}
