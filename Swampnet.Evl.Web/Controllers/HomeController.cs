using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swampnet.Core.Evl;
using Swampnet.Evl.Web.Models;
using Swampnet.Evl.Web.Models.HomeViewModels;

namespace Swampnet.Evl.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(EventSearchCriteriaViewModel criteria = null)
        {
            HomeViewModel vm = null;

            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.ActiveApiKey.HasValue)
            {
                var results = await SearchAsync(user.ActiveApiKey.Value, criteria);

                vm = new HomeViewModel(results, criteria);
            }

            return View(vm);
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private Task<IEnumerable<EventSummary>> SearchAsync(Guid key, EventSearchCriteriaViewModel criteria)
        {
            return GetAsync<IEnumerable<EventSummary>>(key, "events", criteria?.ToQuery());
        }


        private async Task<T> GetAsync<T>(Guid key, string action, string query = null)
        {
            using(var client = new HttpClient())
            {
                var endpoint = "https://swamp-evl.azurewebsites.net"; // from cfg
                client.DefaultRequestHeaders.Add("x-api-key", key.ToString());

                var url = $"{endpoint}/{action}";

                if (!string.IsNullOrEmpty(query))
                {
                    url += "?" + query;
                }

                Debug.WriteLine("Evl: " + url);

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
