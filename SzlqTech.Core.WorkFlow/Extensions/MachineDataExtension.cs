using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.Core.Events;
using SzlqTech.Equipment.Machine;

namespace SzlqTech.Core.WorkFlow.Extensions
{
    public static class MachineDataExtension
    {
        /// <summary>
        /// 更新多语言
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
        /// 注册多语言提示消息 
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
    }
}
