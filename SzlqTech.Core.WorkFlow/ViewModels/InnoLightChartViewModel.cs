
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Data;
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
            Students = new DataTable();
            
        }

        private DataTable students;

        public DataTable Students
        {
            get { return students; }
            set { SetProperty(ref students, value); }
        }


        [RelayCommand]
        private void WinLoaded(object sender)
        {
            if (sender != null)
            {

            }
            if (Students == null || Students.Rows.Count < 1)
            {
                var students = new DataTable();
                students.Columns.Add("Id", typeof(int));
                students.Columns.Add("Name", typeof(string));
                students.Columns.Add("Age", typeof(int));
                students.Columns.Add("Gender", typeof(string));
                students.Columns.Add("Addr", typeof(string));
                var parentName = new string[5] { "张", "王", "李", "赵", "刘" };
                var province = new string[5] { "河南", "江苏", "河北", "湖北", "福建" };

                for (int i = 0; i < 20; i++)
                {
                    var dr = students.NewRow();
                    dr["Id"] = i;
                    dr["Name"] = parentName[(i % 5)] + i.ToString().PadLeft(2, 'A');
                    dr["Age"] = 20 + (i % 5);
                    dr["Gender"] = i % 2 == 0 ? "男" : "女";
                    dr["Addr"] = province[(i % 5)];

                    students.Rows.Add(dr);
                }
                this.Students = students;
            }
        }
    }
}
