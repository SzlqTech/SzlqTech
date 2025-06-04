using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SzlqTech.Core.WorkFlow.Vos
{
    public partial class BaseTraceVo:BaseVo
    {
        [ObservableProperty]
        public string enterTime;

        [ObservableProperty]
        public string exitTime;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string sn;

        /// <summary>
        /// 检测结果
        /// </summary>
        [ObservableProperty]
        public bool result;

    }
}
