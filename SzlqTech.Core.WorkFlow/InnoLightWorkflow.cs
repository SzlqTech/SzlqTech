
using Castle.Windsor.Diagnostics;
using Prism.Events;
using Prism.Ioc;
using SzlqTech.Common.Extensions;
using SzlqTech.Core.Events;
using SzlqTech.Core.WorkFlow.Extensions;
using SzlqTech.Equipment;
using SzlqTech.Equipment.Machine;

namespace SzlqTech.Core.WorkFlow
{
    public class InnoLightWorkflow : BaseWorkFlow
    {
        private readonly IExecutingMachine executingMachine;
        private IEventAggregator aggregator;
        public event EventHandler<TEventArgs<PLCData>>? PLCDataReceived;

        public InnoLightWorkflow(IExecutingMachine ExecutingMachine, IExecutingScanner ExecutingScanner) : base(ExecutingMachine, ExecutingScanner)
        {
            aggregator=ContainerLocator.Container.Resolve<IEventAggregator>();
            executingMachine = ExecutingMachine;
            executingMachine.PLCDataReceived -= ExecutingMachine_PLCDataReceived;
            executingMachine.PLCDataReceived += ExecutingMachine_PLCDataReceived;
        }

        private void ExecutingMachine_PLCDataReceived(object? sender, Common.Extensions.TEventArgs<PLCData> e)
        {
            if(e.Data == null) return;  
            RaisePLCDataReceived(e.Data);
        }

        public void RaisePLCDataReceived(PLCData data)
        {
            PLCDataReceived?.Invoke(this, new Common.Extensions.TEventArgs<PLCData>(data));
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
