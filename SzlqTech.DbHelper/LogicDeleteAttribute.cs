

namespace SzlqTech.DbHelper
{
    public class LogicDeleteAttribute : Attribute
    {
        public bool LogicDelete { get; set; }

        public LogicDeleteAttribute(bool logicDelete)
        {
            LogicDelete = logicDelete;
        }
    }
}
