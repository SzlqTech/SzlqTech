using AutoMapper;
using SzlqTech.Core.WorkFlow.Vos;
using SzlqTech.Entity;

namespace SzlqTech.Core.WorkFlow.AutoMapper
{
    public class InnoTraceProfile:Profile
    {
        public InnoTraceProfile()
        {         
            CreateMap<DataCollect,DataCollectVo>(). ReverseMap();           
        }
    }
}
