

using Newtonsoft.Json;

namespace SzlqTech.Core.WorkFlow.Vos
{
    public class DataCollectModel
    {
        public string Key { get; set; }

        public dynamic? Value { get; set; }

        [JsonIgnore]
        public string MachineName { get; set; }

        [JsonIgnore]
        public long MachineId { get; set; }
    }
}
