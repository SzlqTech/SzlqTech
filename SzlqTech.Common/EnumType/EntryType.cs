
using System.ComponentModel;


namespace SzlqTech.Common.EnumType
{
    public enum EntryType
    {
        /// <summary>
        /// 菜单目录
        /// </summary>
        // [LocalizableDescription(DescriptionConstant.Catalog)]
        [Description("目录")]
        Catalog,
        /// <summary>
        /// 可打开标签页的菜单
        /// </summary>
        // [LocalizableDescription(DescriptionConstant.Menu)]
        [Description("菜单")]
        Menu,
        // [LocalizableDescription(DescriptionConstant.Module)]
        [Description("模块")]
        Module,
        /// <summary>
        /// 工具栏按钮
        /// </summary>
        // [LocalizableDescription(DescriptionConstant.Button)]
        [Description("按钮")]
        Button,
        /// <summary>
        /// 数据表格操作按钮
        /// </summary>
        [Description("操作")]
        Operate
    }
}
