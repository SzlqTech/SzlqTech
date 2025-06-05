
using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SqlqTech.SharedView.Vo
{
    public partial class MachineSettingVo:BaseVo
    {
        [ObservableProperty]
        public int machineModel;


        // public MachineModel MachineModelEnum;
        [ObservableProperty]
        public string portKey;

        [ObservableProperty]
        public string portName;


        [ObservableProperty]
        public string selectedMachineType;


    }
}
