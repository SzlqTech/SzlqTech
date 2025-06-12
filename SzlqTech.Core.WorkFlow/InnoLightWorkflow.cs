
using Castle.Windsor.Diagnostics;
using Prism.Events;
using Prism.Ioc;
using SzlqTech.Core.Events;
using SzlqTech.Core.WorkFlow.Extensions;
using SzlqTech.Equipment;
using SzlqTech.Equipment.Machine;

namespace SzlqTech.Core.WorkFlow
{
    public class InnoLightWorkflow : BaseWorkFlow
    {
        private IEventAggregator aggregator;

        public InnoLightWorkflow(IExecutingMachine ExecutingMachine, IExecutingScanner ExecutingScanner) : base(ExecutingMachine, ExecutingScanner)
        {
             aggregator=ContainerLocator.Container.Resolve<IEventAggregator>();
        }

        public override void OnExecutingMachineDataReceived(MachineData data)
        {
            MachineDataModel model=new MachineDataModel()
            {
                MachineData = data,
                Filter = "InnoLightTraceViewModel"
            };
            aggregator.SendMachineDataModel(model);       
        }
    }
}
