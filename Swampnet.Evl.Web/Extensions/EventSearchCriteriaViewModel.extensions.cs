using Swampnet.Evl.Web.Models;
using System;
using System.Text;
using System.Web;

namespace Swampnet.Evl.Web
{
    public static class EventSearchCriteriaViewModelExtensions
    {
        public static string ToQuery(this EventSearchCriteriaViewModel criteria)
        {
            var query = new StringBuilder($"page={criteria.Page}&pageSize={criteria.PageSize + 1}"); // Always try to get 1 more result than required so we can tell if there's more data available

            string categories = "";

            if (!string.IsNullOrEmpty(categories))
            {
                query.AppendFormat("&categories={0}", categories);
            }


            if (!string.IsNullOrEmpty(criteria.Summary))
            {
                query.AppendFormat("&summary={0}", HttpUtility.HtmlEncode(criteria.Summary));
            }
            if (!string.IsNullOrEmpty(criteria.Source))
            {
                query.AppendFormat("&source={0}", HttpUtility.HtmlEncode(criteria.Source));
            }
            if (!string.IsNullOrEmpty(criteria.Tags))
            {
                query.AppendFormat("&tags={0}", HttpUtility.HtmlEncode(criteria.Tags));
            }

            if (criteria.FromDate.HasValue)
            {
                query.AppendFormat("&fromUtc={0:yyyy-MM-dd} {1:00}:00", criteria.FromDate.Value, criteria.FromHour);
            }
            // No from/to date specified: Default to last 24 hours if we have no other criteria
            else if (!criteria.ToDate.HasValue && !criteria.HasCriteria)
            {
                query.AppendFormat("&fromUtc={0:yyyy-MM-dd} {1:00}:00", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.Hour - 1);
            }

            if (criteria.ToDate.HasValue)
            {
                // Specified a [to] with no [from]. Use 24 hours before [to]
                if (!criteria.FromDate.HasValue)
                {
                    query.AppendFormat("&fromUtc={0:yyyy-MM-dd} {1:00}:00", criteria.ToDate.Value.AddDays(-1), criteria.ToHour);
                }
                query.AppendFormat("&toUtc={0:yyyy-MM-dd} {1:00}:00", criteria.ToDate.Value, criteria.ToHour);
            }

            return query.ToString();
        }

    }
}
