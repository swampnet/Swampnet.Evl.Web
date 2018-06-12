using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swampnet.Evl.Web.Models;
using Swampnet.Evl.Web.Services;

namespace Swampnet.Evl.Web.Controllers
{
    [Authorize]
    public class RulesController : Controller
    {
        private readonly IEvlApi _api;
        private readonly UserManager<ApplicationUser> _userManager;

        public RulesController(UserManager<ApplicationUser> userManager, IEvlApi api)
        {
            _api = api;
            _userManager = userManager;
        }


        // Return all rules
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var rules = await _api.RulesAsync(user.ActiveApiKey.Value);

            return View(rules);
        }


        public IActionResult Details(Guid id)
        {
            return View();
        }


        [HttpPost]
        public IActionResult Save(RuleViewModel rule)
        {
            return Ok();
        }


        #region API

        [HttpGet("rules/details/{id}/json")]
        public async Task<IActionResult> RuleData(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var rule = await _api.RuleAsync(user.ActiveApiKey.Value, id);

            return Json(rule);
        }

        #endregion
    }
}