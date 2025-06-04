using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Reflection;


namespace SzlqTech.Common.Helper
{
    public static class ExcelHelper<T> where T : class,new()
    {
        /// 将数据导出到Excel文件
        /// </summary>
        /// <param name="data">要导出的数据集合</param>
        /// <param name="headers">表头标题（与属性名对应）</param>
        /// <param name="propertyNames">要导出的属性名</param>
        /// <param name="filePath">保存路径</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="isXlsx">是否为XLSX格式（否则为XLS格式）</param>
        public static void ExportToFile(List<T> data, Dictionary<string, string> headers,
                                 List<string> propertyNames, string filePath,
                                 string sheetName = "Sheet1", bool isXlsx = true)
        {
            var workbook = CreateWorkbook(isXlsx);
            var sheet = workbook.CreateSheet(sheetName);

            // 创建表头
            var headerRow = sheet.CreateRow(0);
            for (int i = 0; i < propertyNames.Count; i++)
            {
                string propertyName = propertyNames[i];
                string headerText = headers.ContainsKey(propertyName) ? headers[propertyName] : propertyName;
                headerRow.CreateCell(i).SetCellValue(headerText);
            }

            // 填充数据
            for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
            {
                var row = sheet.CreateRow(rowIndex + 1);
                T item = data[rowIndex];

                for (int colIndex = 0; colIndex < propertyNames.Count; colIndex++)
                {
                    string propertyName = propertyNames[colIndex];
                    PropertyInfo property = typeof(T).GetProperty(propertyName);

                    if (property != null)
                    {
                        object value = property.GetValue(item);
                        string cellValue = value != null ? value.ToString() : string.Empty;
                        row.CreateCell(colIndex).SetCellValue(cellValue);
                    }
                }
            }

            // 自动调整列宽
            for (int i = 0; i < propertyNames.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            // 保存文件
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }

        /// <summary>
        /// 创建工作簿（根据格式选择XLSX或XLS）
        /// </summary>
        private static IWorkbook CreateWorkbook(bool isXlsx)
        {
            return isXlsx ? (IWorkbook)new XSSFWorkbook() : new HSSFWorkbook();
        }
    }
}
