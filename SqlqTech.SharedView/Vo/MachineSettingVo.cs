
using SzlqTech.Common.EnumType;
using SzlqTech.Entity;

namespace SqlqTech.SharedView.Vo
{
    public class MachineSettingVo:BaseVo
    {     
        public int MachineModel { get; set; }


        public MachineModel MachineModelEnum;

        public string PortKey;

        
        public string PortName { get; set; } = null!;
       
        public string? Description { get; set; }
    }
}
