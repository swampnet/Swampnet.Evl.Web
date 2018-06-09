using Swampnet.Core.Evl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<EventSummary> results, EventSearchCriteriaViewModel criteria)
        {
            Criteria = criteria;

            Results = results?.Select(r => new EventSummaryViewModel() {
                Id = r.Id,
                Summary = r.Summary,
                TimestampUtc = r.TimestampUtc
            });
        }

        public EventSearchCriteriaViewModel Criteria { get; private set; }

        public IEnumerable<EventSummaryViewModel> Results { get; private set; }
    }
}
