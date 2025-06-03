using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.Core.Events
{
    public class LocalizationModel
    {
        public bool IsUpdate { get; set; }

        public string Filter { get; set; }
    }

    public class LocalizationEvent:PubSubEvent<LocalizationModel>
    {

    }
}
