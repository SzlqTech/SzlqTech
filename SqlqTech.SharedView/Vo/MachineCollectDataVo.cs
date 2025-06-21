using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SqlqTech.SharedView.Vo
{
    public partial class MachineCollectDataVo:BaseVo
    {
        /// <summary>
        /// 关联的PLC表的ID
        /// </summary>
        [ObservableProperty]
        public long machineId;

        /// <summary>
        /// 端口键
        /// </summary
        [ObservableProperty]
        public string portKey;


        [ObservableProperty]
        public string headerTitle;

        [ObservableProperty]
        public string bindingName;


        [ObservableProperty]
        public bool isEnable;

        [ObservableProperty]
        public bool isSysDate;
    }
}
