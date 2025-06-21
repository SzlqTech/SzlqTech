
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Windows.Controls;
using System.Windows.Data;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.WorkFlow.Views;
using SzlqTech.Localization;


namespace SzlqTech.Core.WorkFlow.ViewModels
{
    public partial class InnoLightChartViewModel:NavigationViewModel
    {
        public InnoLightChartViewModel()
        {
            Title = LocalizationService.GetString(AppLocalizations.ChartView);
            Init();
        }


        [ObservableProperty]
        public ObservableCollection<ExpandoObject> students;

        public DataGrid dataGrid;

        [RelayCommand]
        public void WinLoaded(object sender)
        {
            if (sender != null)
            {
                var control = sender as InnoLightChartView;
                if (control != null)
                {
                    this.dataGrid = control.dgTest;
                }
            }
            if (Students == null || Students.Count < 1)
            {
                var parentName = new string[5] { "张", "王", "李", "赵", "刘" };
                var province = new string[5] { "河南", "江苏", "河北", "湖北", "福建" };
                //for (int i = 0; i < 20; i++)
                //{
                //    dynamic item = new ExpandoObject();
                    
                //    item.Id = i.ToString();
                //    item.Name = parentName[(i % 5)] + i.ToString().PadLeft(2, 'A');
                //    item.Age = 20 + (i % 5);
                //    item.Gender = i % 2 == 0 ? "男" : "女";
                //    item.Addr = province[(i % 5)];
                //    this.Students.Add(item);
                //}
                //添加列
                this.dataGrid.Columns.Add(new DataGridTextColumn() { Header = "学号", Binding = new Binding("Id") });
                this.dataGrid.Columns.Add(new DataGridTextColumn() { Header = "姓名", Binding = new Binding("Name") });
                this.dataGrid.Columns.Add(new DataGridTextColumn() { Header = "年龄", Binding = new Binding("Age") });
                this.dataGrid.Columns.Add(new DataGridTextColumn() { Header = "性别", Binding = new Binding("Gender") });
                this.dataGrid.Columns.Add(new DataGridTextColumn() { Header = "地址", Binding = new Binding("Addr") });
            }
        }

        private void Init()
        {
            Students = new ObservableCollection<ExpandoObject>();
        }

    }
}
