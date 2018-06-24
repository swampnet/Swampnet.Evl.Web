using Swampnet.Core.Evl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<EventSummary> results, EventSearchCriteriaViewModel criteria, string timezone)
        {
            Criteria = criteria;
            Results = results?.Select(r => new EventSummaryViewModel() {
                Id = r.Id,
                Summary = r.Summary,
                TimestampUtc = r.TimestampUtc,
                Category = r.Category,
                Source = r.Source,
                Tags = r.Tags
            });
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
        }

        public EventSearchCriteriaViewModel Criteria { get; private set; }

        public IEnumerable<EventSummaryViewModel> Results { get; private set; }

        public TimeZoneInfo TimeZone { get; private set; }
        
    }
}
