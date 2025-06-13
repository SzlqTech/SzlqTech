

using AutoMapper;
using SqlqTech.Core.Vo;
using SqlqTech.SharedView.Vo;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;

namespace SzlqTech.Services.Mapper
{
   public class AutoMapperProfile: Profile
   {
        public AutoMapperProfile()
        {
            CreateMap<MachineSetting, MachineSettingVo>().ForMember(dest => dest.PortKey, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest=>dest.PortKey,opt=>opt.MapFrom(src=>src.PortKey)).ReverseMap();
            CreateMap<MachineDetail, MachineDetailVo>().ForMember(dest => dest.ScanCycleValue, opt => opt.MapFrom(src => src.ScanCycle))           
                .ForMember(dest=>dest.IsEnableScan,opt=>opt.MapFrom(src=>src.StatusEnable))
                .ReverseMap();
            CreateMap<ScannerSetting, ScannerSettingVo>().ReverseMap();
            CreateMap<Product,ProductVo>().ReverseMap();
            CreateMap<QrCode,QrCodeVo>().ReverseMap();
            CreateMap<SysUser,SysUserVo>().ReverseMap();
            CreateMap<SysRole, SysRoleVo>().ReverseMap();
            CreateMap<SysMenu, SysMenuVo>().ReverseMap();
            CreateMap<SysRoleMenuVo, SysRoleMenu>().ReverseMap();
        }
    }
}
