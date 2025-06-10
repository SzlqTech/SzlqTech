
using SqlSugar;
using SzlqTech.Common.EnumType;


namespace SzlqTech.Entity
{
    /// <summary>
    /// 菜单页面
    /// </summary>
    [Serializable]
    [SugarTable("sys_menu", TableDescription = "系统菜单表")]
    public class SysMenu : BaseAuditableEntity
    {
        /// <summary>
        /// 父Id
        /// </summary>
        [SugarColumn(ColumnName = "parent_id", IsNullable = true, ColumnDescription = "父菜单id")]
        public long? ParentId { set; get; }

        /// <summary>
        /// 根Id
        /// </summary>
        [SugarColumn(ColumnName = "root_id", ColumnDescription = "根菜单id")]
        public long RootId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
     
        [SugarColumn(ColumnName = "text", IsNullable = true, ColumnDescription = "导航页面")]
        public string? Text { set; get; }

        [SugarColumn(ColumnName = "title", IsNullable = true, ColumnDescription = "标题")]
        public string? Title { set; get; }

        [SugarColumn(ColumnName = "text_en", IsNullable = true, ColumnDescription = "英文标题")]
        public string? TextEN { get; set; }

        [SugarColumn(ColumnName = "text_zh", IsNullable = true, ColumnDescription = "中文标题")]
        public string? TextZH { get; set; }

        /// <summary>
        /// 程序集
        /// </summary>
       
        [SugarColumn(ColumnName = "assembly", IsNullable = true, ColumnDescription = "程序集")]
        public string? Assembly { set; get; }

        /// <summary>
        /// 地址
        /// </summary>
       
        [SugarColumn(ColumnName = "url", IsNullable = true, ColumnDescription = "地址")]
        public string? Url { set; get; }

        /// <summary>
        /// 按钮命令
        /// </summary>
       
        [SugarColumn(ColumnName = "command", IsNullable = true, ColumnDescription = "按钮命令")]
        public string? Command { set; get; }

        //        /// <summary>
        //        /// 层级
        //        /// </summary>
        //        [Description("层级")]
        //        public int? Level { set; get; }

        /// <summary>
        /// 类型
        /// </summary>
       
        [SugarColumn(ColumnName = "entry_type", ColumnDescription = "类型")]
        public EntryType EntryType { set; get; }

        /// <summary>
        /// 可见
        /// </summary>
       
        [SugarColumn(ColumnName = "visible", ColumnDescription = "可见")]
        public bool Visible { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
      
        [SugarColumn(ColumnName = "ordinal", ColumnDescription = "序号")]
        public int Ordinal { set; get; } = 0;

        [SugarColumn(ColumnName = "view", ColumnDescription = "页面名称",IsNullable =true)]
        public string? View {
            get
            {
                if (string.IsNullOrEmpty(Url) || !Url.Contains("."))
                {
                    return string.Empty;
                }
                string[] strs = Url.Split('.');
                return strs[strs.Length - 1];              
            }
        }

        [SugarColumn(ColumnName = "icon", ColumnDescription = "图标", IsNullable = true)]
        public string? Icon { get; set; }

        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色Id", IsNullable = true)]
        public long RoleId { get; set; }
        //parent_icon
        [SugarColumn(ColumnName = "parent_icon", ColumnDescription = "父Icon", IsNullable = true)]
        public string? ParentIcon { get; set; }
        // [SugarColumn(ColumnName = "icon")]
        // public byte[]? Icon { get; set; }


        [SugarColumn(IsIgnore = true)]
        public string? ParentText { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string? RootText { get; set; }

        [SugarColumn(IsIgnore = true)]
        public override string Code { get; set; } = string.Empty;

        //
        // [SugarColumn(IsIgnore = true)]
        // public List<SysMenu> SubSysMenus { get; set; }
        //
        // [SugarColumn(IsIgnore = true)]
        // public bool IsExpended { get; set; }
        //
        //
        // [SugarColumn(IsIgnore = true)]
        // public bool IsChecked { get; set; }
        //
        // [SugarColumn(IsIgnore = true)]
        // public string Name => $"[{EntryType.GetAttributeOfType<DescriptionAttribute>().Description}]-{Text}";

        public override string ToString()
        {
            return $"{Text}-[{Url}]";
        }
    }
}