using CommunityToolkit.Mvvm.ComponentModel;


namespace SqlqTech.SharedView.Vo
{
    public partial class MachineTypeVo:ObservableObject
    {
        [ObservableProperty]
        public int machineType;

        [ObservableProperty]
        public string machineTypeName;
    }
}
