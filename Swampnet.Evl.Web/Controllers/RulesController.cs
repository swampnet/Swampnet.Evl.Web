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

        public IActionResult Index()
        {
            return View();
        }


        #region API

        [HttpGet("rules/json")]
        public async Task<IActionResult> RuleData()
        {
            var user = await _userManager.GetUserAsync(User);
            var rules = await _api.RulesAsync(user.ActiveApiKey.Value);

            return Json(rules);
        }

        #endregion
    }
}