using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Swampnet.Core.Evl;
using Swampnet.Evl.Web.Models;
using Swampnet.Evl.Web.Models.HomeViewModels;
using Swampnet.Evl.Web.Services;

namespace Swampnet.Evl.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEvlApi _evl;

        public HomeController(UserManager<ApplicationUser> userManager, IEvlApi evl)
        {
            _userManager = userManager;
            _evl = evl;
        }


        public async Task<IActionResult> Index(EventSearchCriteriaViewModel criteria = null)
        {
            HomeViewModel vm = null;

            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.ActiveApiKey.HasValue)
            {
                var results = await _evl.SearchAsync(user.ActiveApiKey.Value, criteria);

                vm = new HomeViewModel(results, criteria);
            }

            return View(vm);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            var vm = await _evl.DetailsAsync(user.ActiveApiKey.Value, id);

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


    }
}
