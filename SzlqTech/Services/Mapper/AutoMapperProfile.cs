

using AutoMapper;
using SqlqTech.SharedView.Vo;
using SzlqTech.Entity;

namespace SzlqTech.Services.Mapper
{
   public class AutoMapperProfile: Profile
   {
        public AutoMapperProfile()
        {
            CreateMap<MachineSetting, MachineSettingVo>().ReverseMap();
            CreateMap<MachineDetail, MachineDetailVo>().ForMember(dest => dest.ScanCycleValue, opt => opt.MapFrom(src => src.ScanCycle))           
                .ForMember(dest=>dest.IsEnableScan,opt=>opt.MapFrom(src=>src.StatusEnable))
                .ReverseMap();
            CreateMap<ScannerSetting, ScannerSettingVo>().ReverseMap();
            CreateMap<Product,ProductVo>().ReverseMap();
        }
    }
}
