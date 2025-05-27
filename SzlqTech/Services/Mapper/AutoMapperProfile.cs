

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
        }
    }
}
