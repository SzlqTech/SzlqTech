using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.Core.Vos
{
    public class MenuNodesVo : BindableBase
    {
        public MenuNodesVo(string title, params MenuNodeItem[] items)
        {
            Title = title;
            MenuNodeItems = new ObservableCollection<MenuNodeItem>(items);
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 全部选择
        /// </summary>
        private bool isSelectedAll;

        /// <summary>
        /// 全部选择
        /// </summary>
        public bool IsSelectedAll
        {
            get { return isSelectedAll; }
            set { isSelectedAll = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// item元素被选中
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// item元素被选中
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<MenuNodeItem> MenuNodeItems { get; set; } = new ObservableCollection<MenuNodeItem>();
    }
}
