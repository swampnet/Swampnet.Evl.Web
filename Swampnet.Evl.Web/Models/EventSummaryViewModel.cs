using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swampnet.Core.Evl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models
{
    public class EventSummaryViewModel
    {
        public Guid Id { get; set; }

        public DateTime TimestampUtc { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EventCategory Category { get; set; }

        public string Source { get; set; }

        public string Summary { get; set; }

        public string[] Tags { get; set; }
    }
}
