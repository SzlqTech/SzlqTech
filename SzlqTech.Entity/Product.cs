using SqlSugar;

namespace SzlqTech.Entity
{
    [SugarTable("product", TableDescription = "产品")]
    public class Product:BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "product_name", IsNullable = true, ColumnDescription = "产品名称")]
        public string ProductName { get; set; }

        [SugarColumn(ColumnName = "product_code", IsNullable = true, ColumnDescription = "产品代码")]
        public string ProductCode { get; set; }
    }
}
