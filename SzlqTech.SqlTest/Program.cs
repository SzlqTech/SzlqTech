using SqlSugar;
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using DbType = SqlSugar.DbType;

string connStr = DbAndApiAuthConfig.Config.DbConnectionString;
DbType dbType = DbAndApiAuthConfig.Config.DbType;
SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
{
    ConnectionString = connStr,
    DbType = dbType,
    IsAutoCloseConnection = true, 
    
});

db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysUser));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(MachineSetting));
