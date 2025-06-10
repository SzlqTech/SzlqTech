
using SzlqTech.Common.EnumType;

namespace SzlqTech.Common.Views
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class ViewAttribute : Attribute
    {
        public ViewAttribute(string text)
            : this(text, EntryType.Menu)
        {
        }

        public ViewAttribute(string text, bool status)
            : this(text, EntryType.Menu, null, null, 0, null, null, status)
        {
        }

        public ViewAttribute(string text, string parent)
            : this(text, EntryType.Menu, parent, parent, 0, null, null, true)
        {
        }

        public ViewAttribute(string text,string title, string parent, string Icon)
           : this(text,title, EntryType.Menu, parent, parent, 0, Icon, null, true)
        {
        }

        public ViewAttribute(string text, string title, string parent, string Icon,string parentIcon)
         : this(text, title, EntryType.Menu, parent, parent, 0, Icon,parentIcon, null, true)
        {
        }

        public ViewAttribute(string text, string parent, string Icon)
            : this(text, EntryType.Menu, parent, parent, 0, Icon, null, true)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="text">菜单标题</param>
        /// <param name="parent">父目录标题</param>
        /// <param name="ordinal">排序</param>
        public ViewAttribute(string text, string parent, int ordinal)
            : this(text, EntryType.Menu, parent, parent, ordinal, null, null, true)
        {
        }


        public ViewAttribute(string text, string parent, string root, int ordinal)
            : this(text, EntryType.Menu, parent, root, ordinal, null, null, true)
        {
        }

        public ViewAttribute(string text, EntryType entryType, string parent)
            : this(text, entryType, parent, null, 0, null, null, true)
        {
        }

        public ViewAttribute(string text, EntryType entryType)
            : this(text, entryType, null, null, 0, null, null, true)
        {
        }

        public ViewAttribute(string text, EntryType entryType, int ordinal)
            : this(text, entryType, null, null, ordinal, null, null, true)
        {
        }
        public ViewAttribute(string text, EntryType entryType, string parent, string root)
            : this(text, entryType, parent, root, 0)
        {
        }
        public ViewAttribute(string text, EntryType entryType, string parent, string root, int ordinal)
            : this(text, entryType, parent, root, ordinal, null, null, true)
        {
        }
        public ViewAttribute(string text, EntryType entryType, string? parent, string? root, int ordinal,
            string? description, string? toolTip, bool status)
        {
            this.Text = text;
            this.EntryType = entryType;
            this.Parent = parent;
            this.Root = root;
            this.Ordinal = ordinal;
            this.Description = description;
            this.ToolTip = toolTip;
            this.Status = status;
        }

        public ViewAttribute(string title,string text, EntryType entryType, string? parent, string? root, int ordinal,
            string? description, string? toolTip, bool status)
        {
            this.Text = text;
            this.Title = title;
            this.EntryType = entryType;
            this.Parent = parent;
            this.Root = root;
            this.Ordinal = ordinal;
            this.Description = description;
            this.ToolTip = toolTip;
            this.Status = status;
        }

        public ViewAttribute(string title, string text, EntryType entryType, string? parent, string? root, int ordinal,
            string? description,string parentIcon, string? toolTip, bool status)
        {
            this.Text = text;
            this.Title = title;
            this.EntryType = entryType;
            this.Parent = parent;
            this.Root = root;
            this.Ordinal = ordinal;
            this.Description = description;
            this.ToolTip = toolTip;
            this.Status = status;
            this.ParentIcon = parentIcon;
        }

        /// <summary>
        /// 导航页面
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 标题名称
        /// </summary>
        public string Title { get; set; }

        public string? TextEN { get; set; }

        public string? TextZH { get; set; }


        public EntryType EntryType { get; }

        public string? Parent { get; set; }

        public string? ParentEN { get; set; }

        public string? ParentZH { get; set; }

        public string? Root { get; set; }

        public string? RootEN { get; set; }

        public string? RootZH { get; set; }

        public int Ordinal { get; set; }

        public string? Description { get; set; }

        public string ? ParentIcon { get; set; }

        public string? ToolTip { get; set; }

        /// <summary>
        /// 暂时没有用
        /// </summary>
        public bool Status { get; set; }
    }
}
