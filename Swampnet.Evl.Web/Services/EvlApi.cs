using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Swampnet.Core.Evl;
using Swampnet.Evl.Web.Models;
using Swampnet.Evl.Web.Models.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Services
{
    public interface IEvlApi
    {
        Task<IEnumerable<EventSummary>> SearchAsync(Guid key, EventSearchCriteriaViewModel criteria);
        Task<EventDetailsViewModel> DetailsAsync(Guid key, Guid id);
        Task<IEnumerable<RuleSummaryViewModel>> RulesAsync(Guid key);
        Task<RuleViewModel> RuleAsync(Guid key, Guid id);
        Task<RuleViewModel> CreateRuleAsync(Guid key);
        Task UpdateRuleAsync(Guid key, RuleViewModel rule);
    }


    public class EvlApi : IEvlApi
    {
        private readonly IConfiguration _cfg;

        public EvlApi(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public Task<IEnumerable<EventSummary>> SearchAsync(Guid key, EventSearchCriteriaViewModel criteria)
        {
            return GetAsync<IEnumerable<EventSummary>>(key, "events", criteria?.ToQuery());
        }

        public Task<EventDetailsViewModel> DetailsAsync(Guid key, Guid id)
        {
            return GetAsync<EventDetailsViewModel>(key, $"events/{id}");
        }

        public Task<IEnumerable<RuleSummaryViewModel>> RulesAsync(Guid key)
        {
            return GetAsync<IEnumerable<RuleSummaryViewModel>>(key, $"rules");
        }

        public Task<RuleViewModel> RuleAsync(Guid key, Guid id)
        {
            return GetAsync<RuleViewModel>(key, $"rules/{id}");
        }

        public Task<RuleViewModel> CreateRuleAsync(Guid key)
        {
            return PostAsync<RuleViewModel>(key, "rules");
        }

        public Task UpdateRuleAsync(Guid key, RuleViewModel rule)
        {
            return PutAsync(key, $"rules/{rule.Id}", rule);
        }


        private async Task PutAsync(Guid key, string action, object payload)
        {
            using (var client = new HttpClient())
            {
                var endpoint = _cfg["evl:endpoint"];
                client.DefaultRequestHeaders.Add("x-api-key", key.ToString());

                var url = $"{endpoint}/{action}";

                var rs = await client
                    .PutAsync(url, new StringContent(
                        JsonConvert.SerializeObject(payload),
                        Encoding.UTF8,
                        "application/json"))
                    .ConfigureAwait(false);

                rs.EnsureSuccessStatusCode();

                var json = await rs.Content.ReadAsStringAsync();
            }

        }

        private async Task<T> PostAsync<T>(Guid key, string action, object payload = null)
        {
            using (var client = new HttpClient())
            {
                var endpoint = _cfg["evl:endpoint"];
                client.DefaultRequestHeaders.Add("x-api-key", key.ToString());

                var url = $"{endpoint}/{action}";

                var rs = await client
                    .PostAsync(url, payload == null 
                        ? null 
                        : new StringContent(
                            JsonConvert.SerializeObject(payload),
                            Encoding.UTF8,
                            "application/json"))
                    .ConfigureAwait(false);

                rs.EnsureSuccessStatusCode();

                var json = await rs.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }

        }


        private async Task<T> GetAsync<T>(Guid key, string action, string query = null)
        {
            using (var client = new HttpClient())
            {
                var endpoint = _cfg["evl:endpoint"];
                client.DefaultRequestHeaders.Add("x-api-key", key.ToString());

                var url = $"{endpoint}/{action}";

                if (!string.IsNullOrEmpty(query))
                {
                    url += "?" + query;
                }

                var rs = await client
                    .GetAsync(url)
                    .ConfigureAwait(false);

                rs.EnsureSuccessStatusCode();

                var json = await rs.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
