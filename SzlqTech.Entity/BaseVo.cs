

using CommunityToolkit.Mvvm.ComponentModel;

namespace SzlqTech.Entity
{
    public partial class BaseVo: ObservableObject
    {
        [ObservableProperty]
        public long? createUser;


        [ObservableProperty]
        public DateTime? createTime;


        [ObservableProperty]
        public long? updateUser;

        [ObservableProperty]
        public DateTime? updateTime;

        [ObservableProperty]
        public  int status;


        
        public virtual bool statusEnable
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

        [ObservableProperty]
        public int deleted;

        [ObservableProperty]
        public string? remark;

        [ObservableProperty]
        public string code;

        [ObservableProperty]
        public bool isSelected;

        [ObservableProperty]
        public long id;

        [ObservableProperty]
        public string? description;
    }
}
