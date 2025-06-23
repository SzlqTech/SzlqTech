using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.Equipment.Machine;

namespace SzlqTech.Core.Events
{
    public class MachineStatusModel
    {
        public bool IsOpen { get; set; }

        public string Filter { get; set; }
    }

    public class MachineStatusEvent:PubSubEvent<MachineStatusModel>
    {

    }
}
