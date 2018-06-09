using Swampnet.Core.Evl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<EventSummary> results)
        {
            Results = results?.Select(r => new EventSummaryViewModel() {
                Id = r.Id,
                Summary = r.Summary,
                TimestampUtc = r.TimestampUtc
            });
        }

        public IEnumerable<EventSummaryViewModel> Results { get; private set; }
    }
}
