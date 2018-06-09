using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swampnet.Core.Evl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models.HomeViewModels
{
    public class EventDetailsViewModel
    {
        public Guid Id { get; set; }

        public DateTime TimestampUtc { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EventCategory Category { get; set; }

        public string Summary { get; set; }

        public List<Property> Properties { get; set; }

        public string Source { get; set; }

        public string SourceVersion { get; set; }

        public List<string> Tags { get; set; }

        public List<Trigger> Triggers { get; set; }

        public override string ToString()
        {
            return $"{TimestampUtc:s} [{Source}] [{Category}] {Summary}";
        }
    }

    public class Trigger
    {
        public Trigger()
        {
            TimestampUtc = DateTime.UtcNow;
            Actions = new List<TriggerAction>();
        }

        public Trigger(Guid ruleId, string ruleName)
            : this()
        {
            RuleName = ruleName;
            RuleId = ruleId;
        }

        public DateTime TimestampUtc { get; set; }
        public string RuleName { get; set; }
        public Guid RuleId { get; set; }
        public List<TriggerAction> Actions { get; set; }
    }


    public class TriggerAction
    {
        public DateTime TimestampUtc { get; set; }
        public string Type { get; set; }
        public List<Property> Properties { get; set; }
        public string Error { get; set; }
    }
}
