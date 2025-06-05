
using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SzlqTech.Core.Vos
{
    public partial class QrCodeVo:BaseVo
    {

        /// <summary>
        /// 产品名称
        /// </summary>
        [ObservableProperty]
        public string productName;

        /// <summary>
        /// 产品代码
        /// </summary>
        [ObservableProperty]
        public string productCode;


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
