

namespace SzlqTech.Entity
{
    public class BaseVo
    {
        
        public long? CreateUser { get; set; }

     
        public DateTime? CreateTime { get; set; }

      
        public long? UpdateUser { get; set; }

        
        public DateTime? UpdateTime { get; set; }

      
        public virtual int Status { get; set; }

  
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

       
        public int Deleted { get; set; }

       
        public string? Remark { get; set; }


        public  string Code { get; set; }

     
        public bool IsSelected { get; set; }
    }
}
