

using Prism.Events;

namespace SzlqTech.Core.Events
{
    public class BusyAsyncModel
    {
        public bool IsOpen { get; set; }

        public string Filter { get; set; }
    }

    public class BusyAsyncEvent:PubSubEvent<BusyAsyncModel>
    {

    }
}
