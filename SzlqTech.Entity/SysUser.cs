using SqlSugar;

namespace SzlqTech.Entity
{
    [SugarTable("sys_user", TableDescription = "系统用户")]
    public class SysUser:BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名称")]
        public string UserName { get; set; }

        [SugarColumn(ColumnName = "pass_word", ColumnDescription = "用户密码")]
        public string Password { get; set; }


    }
}
