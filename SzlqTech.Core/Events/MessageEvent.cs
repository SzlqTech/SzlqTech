using Prism.Events;
using SzlqTech.Equipment.Machine;


namespace SzlqTech.Core.Events
{
    public static class MessageEvent
    {
        /// <summary>
        /// 注册提示消息 
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void ResgiterBusyAsyncMessage(this IEventAggregator aggregator,
            Action<BusyAsyncModel> action, string filterName = "Main")
        {
            aggregator.GetEvent<BusyAsyncEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }

        /// <summary>
        /// 发送提示消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendBusyAsyncMessage(this IEventAggregator aggregator, BusyAsyncModel message, string filterName = "Main")
        {
            aggregator.GetEvent<BusyAsyncEvent>().Publish(new BusyAsyncModel()
            {
                Filter = filterName,
                IsOpen = message.IsOpen
            });
        }

        /// <summary>
        /// 注册提示消息 
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void ResgiterSnackBarMessage(this IEventAggregator aggregator,
            Action<SnackBarModel> action, string filterName = "Main")
        {
            aggregator.GetEvent<SnackBarMessageEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }

        /// <summary>
        /// 发送提示消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendSnackBarMessage(this IEventAggregator aggregator, string message, string filterName = "Main")
        {
            aggregator.GetEvent<SnackBarMessageEvent>().Publish(new SnackBarModel()
            {
                Filter = filterName,
                Message = message
            });
        }

        /// <summary>
        /// 更新多语言
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="isUpdate"></param>
        /// <param name="filterName"></param>
        public static void SendUpdateLocalizationModel(this IEventAggregator aggregator, bool isUpdate, string filterName = "Main")
        {
            aggregator.GetEvent<LocalizationEvent>().Publish(new LocalizationModel()
            {
                IsUpdate = isUpdate,
                Filter = filterName,
            });
        }

        /// <summary>
        /// 注册多语言提示消息 
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void ResgiterUpdateLocalizationModel(this IEventAggregator aggregator,
            Action<LocalizationModel> action, string filterName = "Main")
        {
            aggregator.GetEvent<LocalizationEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }
      
    }
}
