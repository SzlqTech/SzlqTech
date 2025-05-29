using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Common.EnumType;
using SzlqTech.Entity;

namespace SqlqTech.SharedView.Vo
{
    public partial class ScannerSettingVo:BaseVo
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        [ObservableProperty]
        public int scannerType;


        /// <summary>
        /// 端口键
        /// </summary>
        [ObservableProperty]
        public string portKey;


        /// <summary>
        /// 端口名称
        /// </summary>
        [ObservableProperty]
        public string portName;

        /// <summary>
        /// 端口描述
        /// </summary>
        [ObservableProperty]
        public string? description;

        /// <summary>
        /// 波特率
        /// </summary>

        public int? baudRate;

        /// <summary>
        /// 编码层级
        /// </summary>
        [ObservableProperty]
        public int codeLevel;

        /// <summary>
        /// 编码格式
        /// </summary>
        [ObservableProperty]
        public int encoding;

        [ObservableProperty]
        public ScannerType scannerModel;

        [ObservableProperty]
        public string? attr0;

        [ObservableProperty]
        public string? attr1;

        [ObservableProperty]
        public string? attr2;

        [ObservableProperty]
        public string? attr3;
    }
}
