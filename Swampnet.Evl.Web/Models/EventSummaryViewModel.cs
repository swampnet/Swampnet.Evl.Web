using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models
{
    public class EventSummaryViewModel
    {
        public Guid Id { get; set; }
        public string Summary { get; set; }
        public DateTime TimestampUtc { get; set; }
    }
}
