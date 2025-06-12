using Prism.Events;
using SzlqTech.Equipment.Machine;

namespace SzlqTech.Core.Events
{
    public class MachineDataModel
    {
        public MachineData MachineData { get; set; }

        public string Filter { get; set; }
    }


    public class MachineDataEvent : PubSubEvent<MachineDataModel>
    {

    }

}
