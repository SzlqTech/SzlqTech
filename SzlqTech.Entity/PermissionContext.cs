

using SzlqTech.Common.Context;
using SzlqTech.Entity;

namespace SSzlqTech.Entity
{
    /// <summary>
    /// 权限上下文
    /// </summary>
    public class PermissionContext : IUserContext
    {
        public long? UserId { get; set; }

        /// <summary>
        /// 用户代码
        /// </summary>
        public string? UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string? PersonName { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long? RoleId { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        public string? RoleCode { get; set; }

        /// <summary>
        /// Session Id
        /// </summary>
        public string? Session { get; set; }

        /// <summary>
        /// 当前用户关联的目录和菜单
        /// </summary>
        private List<SysMenu>? _viewList;

        /// <summary>
        /// 当前用户关联的按钮
        /// </summary>
        private List<SysMenu>? _funcList;

        /// <summary>
        /// 当前用户关联的目录和菜单
        /// </summary>
        public List<SysMenu> ViewList
        {
            get => _viewList ?? (_viewList = new List<SysMenu>());
            set => _viewList = value;
        }

        /// <summary>
        /// 当前用户关联的按钮
        /// </summary>
        public List<SysMenu> FuncList
        {
            get => _funcList ?? (_funcList = new List<SysMenu>());
            set => _funcList = value;
        }

        // public List<SysMenu> OperateList { get; set; } = null!;
    }
}