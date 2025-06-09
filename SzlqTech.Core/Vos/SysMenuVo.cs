
using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SzlqTech.Core.Vos
{
    public partial class SysMenuVo : BaseVo
    {
        [ObservableProperty]
        private string text;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string view;


        [ObservableProperty]
        private string icon;

        [ObservableProperty]
        private string url;

        [ObservableProperty]
        private int status;

        [ObservableProperty]
        private string createTime;

        [ObservableProperty]
        private string updateTime;

        [ObservableProperty]
        private string assembly;

        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private long id;

        [ObservableProperty]
        private long parentId;

        [ObservableProperty]
        private long rootId;

        [ObservableProperty]
        private int entryType;

        [ObservableProperty]
        private bool visible;

        [ObservableProperty]
        private int ordinal;

        [ObservableProperty]
        public long roleId;




    }
}
