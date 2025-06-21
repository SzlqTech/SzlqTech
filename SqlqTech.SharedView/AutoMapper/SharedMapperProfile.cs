using AutoMapper;
using SqlqTech.SharedView.Vo;
using SzlqTech.Entity;


namespace SqlqTech.SharedView.AutoMapper
{
    public class SharedMapperProfile:Profile
    {
        public SharedMapperProfile()
        {
          CreateMap<MachineDataCollect, MachineCollectDataVo>().ReverseMap();

        }
    }
}
