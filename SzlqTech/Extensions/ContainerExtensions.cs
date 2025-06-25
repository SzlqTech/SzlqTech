using AutoMapper;
using Prism.Ioc;
using SqlqTech.SharedView.AutoMapper;
using SzlqTech.Core.Services.App;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.WorkFlow.AutoMapper;
using SzlqTech.Equipment;
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
            container.Register<IProductRepository, ProductRepository>();
            container.Register<IQrCodeRepository, QrCodeRepository>();
            container.Register<ISysUserRepository, SysUserRepository>();
            container.Register<ISysUserService, SysUserServiceImpl>();
            container.Register<ISequenceRepository, SequenceRepository>();
            container.Register<ISysDepartmentRepository, SysDepartmentRepository>();        
            container.Register<ISysDictItemRepository, SysDictItemRepository>();     
            container.Register<ISysDictRepository, SysDictRepository>();     
            container.Register<ISysMenuRepository, SysMenuRepository>();
            container.Register<ISysRoleMenuRepository, SysRoleMenuRepository>();
            container.Register<ISysRoleRepository, SysRoleRepository>();
            container.Register<ISysUserRepository, SysUserRepository>();
            container.Register<ISysUserDetailRepository, SysUserDetailRepository>();
            container.Register<IMachineDataCollectRepostory, MachineDataCollectRepostory>();
            container.Register<IDataCollectRepository,DataCollectRepository>();
        }

        public static void AddDbService(this IContainerRegistry container)
        {
            container.Register<ISysUserService, SysUserServiceImpl>();
            container.Register<IMachineSettingService, MachineSettingServiceImpl>();
            container.Register<IMachineDetailService,MachineDetailServiceImpl>();
            container.Register<IScannerSettingService, ScannerSettingServiceImpl>();
            container.Register<IProductService, ProductServiceImpl>();
            container.Register<IQrCodeService, QrCodeServiceImpl>();
            container.Register<ISysSequenceService, SysSequenceServiceImpl>();
            container.Register<ISysDepartmentService, SysDepartmentServiceImpl>();
            container.Register<ISysDictItemService, SysDictItemServiceImpl>();
            container.Register<ISysDictService, SysDictServiceImpl>();
            container.Register<ISysMenuService, SysMenuServiceImpl>();
            container.Register<ISysRoleMenuService, SysRoleMenuServiceImpl>();
            container.Register<ISysRoleService, SysRoleServiceImpl>();
            container.Register<ISysUserService, SysUserServiceImpl>();
            container.Register<ISysUserDetailService, SysUserDetailServiceImpl>();
            container.Register<IMachineDataCollectService, MachineDataCollectServiceImpl>();
            container.Register<IDataCollectService,DataCollectServiceImpl>();
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
                config.AddProfile(new InnoTraceProfile());
                config.AddProfile(new SharedMapperProfile());
            });
            container.RegisterInstance<IMapper>(config.CreateMapper());
            container.Register<NavigationService>();

            //注册设备服务
            container.Register<IExecutingScanner, ExecutingScanner>();
            container.Register<IExecutingMachine, ExecutingMachine>();
        }
    }
}
