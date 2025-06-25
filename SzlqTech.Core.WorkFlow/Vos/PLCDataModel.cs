using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.Core.WorkFlow.Vos
{
    public class PLCDataModel
    {
        public string PortKey { get; set; }

        public string BindingName { get; set; }

        public string Title { get; set; }

        public dynamic? Value { get; set; }

        public long MachineId { get; set; }

        public string MachineName { get; set; }

      
    }
}
