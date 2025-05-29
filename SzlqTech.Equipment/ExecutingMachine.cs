using SzlqTech.Common.Extensions;
using SzlqTech.Equipment.Machine;

namespace SzlqTech.Equipment
{
    public class ExecutingMachine : IExecutingMachine
    {
        public event EventHandler<TEventArgs<MachineData>>? DataReceived;

        public void CloseMachine()
        {
            
        }

        public void GetMachines()
        {
            
        }

        public bool MainMachineExecute()
        {
            return true;
        }

        public void OpenMachine()
        {
            
        }

        public void RegisterMachineEvent()
        {
            
        }

        public void StartMachine()
        {
            
        }

        public void StopMachine()
        {
            
        }

        public void UnRegisterMachineEvent()
        {
            
        }
    }
}
