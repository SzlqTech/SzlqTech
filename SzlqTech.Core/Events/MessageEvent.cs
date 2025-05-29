using Prism.Events;


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
                IsOpen= message.IsOpen
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
                Message= message
            });
        }
    }
}
