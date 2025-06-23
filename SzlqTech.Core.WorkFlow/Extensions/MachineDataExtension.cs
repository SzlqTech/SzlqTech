using Prism.Events;
using SzlqTech.Core.Events;

namespace SzlqTech.Core.WorkFlow.Extensions
{
    public static class MachineDataExtension
    {
        /// <summary>
        /// 更新MachineData
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="isUpdate"></param>
        /// <param name="filterName"></param>
        public static void SendMachineDataModel(this IEventAggregator aggregator, MachineDataModel data, string filterName = "InnoLightTraceViewModel")
        {
            aggregator.GetEvent<MachineDataEvent>().Publish(new MachineDataModel()
            {
                MachineData = data.MachineData,
                Filter = filterName,
            });
        }

        /// <summary>
        /// 注册MachineData 
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void ResgiterMachineDataModel(this IEventAggregator aggregator,
            Action<MachineDataModel> action, string filterName = "InnoLightTraceViewModel")
        {
            aggregator.GetEvent<MachineDataEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }

        public static void SendMachineStatusModel(this IEventAggregator aggregator, MachineStatusModel data, string filterName = "InnoLightTraceViewModel")
        {
            aggregator.GetEvent<MachineStatusEvent>().Publish(new MachineStatusModel()
            {
                IsOpen=data.IsOpen,
                Filter = filterName,
            });
        }

        public static void ResgiterMachineStatusModel(this IEventAggregator aggregator,
           Action<MachineStatusModel> action, string filterName = "InnoLightTraceViewModel")
        {
            aggregator.GetEvent<MachineStatusEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }
    }
}
