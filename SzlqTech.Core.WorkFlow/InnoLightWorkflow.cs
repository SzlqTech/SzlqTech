
using Castle.Windsor.Diagnostics;
using Prism.Events;
using Prism.Ioc;
using SzlqTech.Common.Extensions;
using SzlqTech.Core.Consts;
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
        public event EventHandler<TEventArgs<List<MachineLinkData>>>? PLCDataReceived;
        public event EventHandler<TEventArgs<bool>>? MachineStatusRecevied;

        public InnoLightWorkflow(IExecutingMachine ExecutingMachine, IExecutingScanner ExecutingScanner) : base(ExecutingMachine, ExecutingScanner)
        {
            aggregator=ContainerLocator.Container.Resolve<IEventAggregator>();
            executingMachine = ExecutingMachine;
            executingMachine.PLCDataReceived -= ExecutingMachine_PLCDataReceived;
            executingMachine.PLCDataReceived += ExecutingMachine_PLCDataReceived;
            executingMachine.MachineStatusReceived -= ExecutingMachine_MachineStatusReceived;
            executingMachine.MachineStatusReceived += ExecutingMachine_MachineStatusReceived;
        }

        private void ExecutingMachine_MachineStatusReceived(object? sender, TEventArgs<bool> e)
        {   
            AppMachineContext.IsOpen = e.Data;
            RaiseMachineStatusRecevied(e.Data);
        }

        private void ExecutingMachine_PLCDataReceived(object? sender, Common.Extensions.TEventArgs<List<MachineLinkData>> e)
        {
            if(e.Data == null) return;  
            RaisePLCDataReceived(e.Data);
        }

        public void RaisePLCDataReceived(List<MachineLinkData> data)
        {
            PLCDataReceived?.Invoke(this, new Common.Extensions.TEventArgs<List<MachineLinkData>>(data));
        }

        public void RaiseMachineStatusRecevied(bool isOpen)
        {
            MachineStatusRecevied?.Invoke(this, new TEventArgs<bool>(isOpen));
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

        public virtual dynamic ReadData(string portKey)
        {
          return  executingMachine.ReadValueByPortKey(portKey);
        }

        public virtual async Task<dynamic> ReadDataAsync(string portKey)
        {
            return await executingMachine.ReadValueByPortKeyAsync(portKey);
        }
    }
}
