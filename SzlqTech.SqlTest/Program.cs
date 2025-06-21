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
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(MachineDetail));
//ScannerSetting SysDepartment SysDict SysDictItem SysMenu SysRole SysRoleMenu SysSequence SysUserDetail
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(ScannerSetting));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(Product));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(QrCode));

db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysDepartment));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysDict));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysDictItem));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysMenu));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysRole));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysRoleMenu));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysSequence));
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(SysUserDetail));
//MachineDataCollect
db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(MachineDataCollect));