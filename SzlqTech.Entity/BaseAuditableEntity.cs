using SqlSugar;


namespace SzlqTech.Entity
{
    //[SugarIndex("index_id", nameof(BaseAuditableEntity.Id), OrderByType.Asc)]
    public class BaseAuditableEntity:BaseEntity
    {
        [SugarColumn(ColumnName = "create_user", IsNullable = true, ColumnDescription = "创建用户id")]
        public long? CreateUser { get; set; }

        [SugarColumn(ColumnName = "create_time", IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreateTime { get; set; }

        [SugarColumn(ColumnName = "update_user", IsNullable = true, ColumnDescription = "更新用户id")]
        public long? UpdateUser { get; set; }

        [SugarColumn(ColumnName = "update_time", IsNullable = true, ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; }

        [SugarColumn(ColumnName = "status", IsNullable = true, ColumnDescription = "业务状态")]
        public virtual int Status { get; set; }

        [SugarColumn(IsIgnore = true)]
        public virtual bool StatusEnable
        {
            get
            {
                return Status > 0;
            }
            set
            {
                Status = (value ? 1 : 0);
            }
        }

        [SugarColumn(ColumnName = "deleted", IsNullable = true, ColumnDescription = "逻辑删除")]
        public int Deleted { get; set; }

        [SugarColumn(ColumnName = "remark", IsNullable = true, ColumnDescription = "备注")]
        public string? Remark { get; set; }

      
        [SugarColumn(ColumnName = "id", ColumnDescription = "主键",IsPrimaryKey =true)]    
        public long Id { get; set; }
    }
}
