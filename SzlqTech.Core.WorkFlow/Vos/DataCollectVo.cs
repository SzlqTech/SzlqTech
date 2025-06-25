using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SzlqTech.Core.WorkFlow.Vos
{
    public partial class DataCollectVo:BaseVo
    {
        /// <summary>
        /// 关联机器ID
        /// </summary>
        [ObservableProperty]
        public long? machineId;

        /// <summary>
        /// 名称
        /// </summary>
        [ObservableProperty]
        public string? name;

        /// <summary>
        /// 是否开启
        /// </summary>
        [ObservableProperty]
        public bool isEnable;

        /// <summary>
        /// 存储键
        /// </summary>
        [ObservableProperty]
        public string key;

        /// <summary>
        /// 存储值
        /// </summary>
        [ObservableProperty]
        public string value;

        /// <summary>
        /// 工站
        /// </summary>
        [ObservableProperty]
        public int station;


        /// <summary>
        /// 工站名称
        /// </summary>
        [ObservableProperty]
        public string stationName;

        /// <summary>
        /// 进入工站时间
        /// </summary>
        [ObservableProperty]
        public string enterDate;

        /// <summary>
        /// 离开工站时间
        /// </summary>
        [ObservableProperty]
        public string leaveDate;
    }
}
