using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.Core.Vos
{
    public class MenuNodeItem : BindableBase
    {

        public MenuNodeItem(string text, string view, string parentName, string icon, long id)
        {
            this.Text = text;
            this.View = view;
            this.Icon = icon;
            this.ParentName = parentName;
            this.Id = id;
        }

        private string parentName;

        public string ParentName
        {
            get { return parentName; }
            set { parentName = value; RaisePropertyChanged(); }
        }


        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; RaisePropertyChanged(); }
        }

        private string view;

        public string View
        {
            get { return view; }
            set { view = value; RaisePropertyChanged(); }
        }

        private string icon;

        public string Icon
        {
            get { return icon; }
            set { icon = value; RaisePropertyChanged(); }
        }


        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; RaisePropertyChanged(); }
        }

        private int status;

        public int Status
        {
            get { return status; }
            set { status = value; RaisePropertyChanged(); }
        }

        private string createTime;

        public string CreateTime
        {
            get { return createTime; }
            set { createTime = value; RaisePropertyChanged(); }
        }

        private string updateTime;

        public string UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; RaisePropertyChanged(); }
        }

        private string assembly;

        public string Assembly
        {
            get { return assembly; }
            set { assembly = value; RaisePropertyChanged(); }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; RaisePropertyChanged(); }
        }

        private long id;

        public long Id
        {
            get { return id; }
            set { id = value; RaisePropertyChanged(); }
        }


        private long parentId;

        public long ParentId
        {
            get { return parentId; }
            set { parentId = value; RaisePropertyChanged(); }
        }

        private long rootId;

        public long RootId
        {
            get { return rootId; }
            set { rootId = value; RaisePropertyChanged(); }
        }

        private int entryType;

        public int EntryType
        {
            get { return entryType; }
            set { entryType = value; RaisePropertyChanged(); }
        }

        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; RaisePropertyChanged(); }
        }

        private int ordinal;

        public int Ordinal
        {
            get { return ordinal; }
            set { ordinal = value; RaisePropertyChanged(); }
        }
    }
}
