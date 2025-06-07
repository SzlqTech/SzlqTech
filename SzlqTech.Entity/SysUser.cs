using SqlSugar;

namespace SzlqTech.Entity
{
    [SugarTable("sys_user", TableDescription = "系统用户")]
    public class SysUser:BaseAuditableEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
      
        [SugarColumn(ColumnName = "username", Length = 50, ColumnDescription = "用户名")]
        public string Username { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(ColumnName = "password", Length = 50, ColumnDescription = "密码")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 姓名
        /// </summary>
       
        [SugarColumn(ColumnName = "person_name", Length = 50, IsNullable = true, ColumnDescription = "姓名")]
        public string? PersonName { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色id")]
        public long RoleId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        
        [SugarColumn(ColumnName = "email", Length = 50, IsNullable = true, ColumnDescription = "邮箱")]
        public string? Email { get; set; }

       

        /// <summary>
        /// 最近登录时间
        /// </summary>
     
        [SugarColumn(ColumnName = "last_login_time", IsNullable = true, ColumnDescription = "最近登录时间")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最近更新密码时间
        /// </summary>
       
        [SugarColumn(ColumnName = "last_update_password_time", IsNullable = true, ColumnDescription = "最近更新密码时间")]
        public DateTime? LastUpdatePasswordTime { get; set; }

        //IsRemember
        /// <summary>
        /// 最近更新密码时间
        /// </summary>
      
        [SugarColumn(ColumnName = "is_remember", IsNullable = true, ColumnDescription = "记住密码")]
        public bool IsRemember { get; set; }


    }
}
