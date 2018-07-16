using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swampnet.Evl.Web.Data;
using Swampnet.Evl.Web.Models;
using Swampnet.Evl.Web.Services;
using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Web.Models.KeyViewModels;
using Microsoft.AspNetCore.Http;

namespace Swampnet.Evl.Web.Controllers
{
    [Authorize]
    public class KeysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEvlApi _evl;

        public KeysController(ApplicationDbContext applicationContext, UserManager<ApplicationUser> userManager, IEvlApi evl)
        {
            _context = applicationContext;
            _userManager = userManager;
            _evl = evl;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var apiKeys = await _context.ApiKeys.Where(k => k.User.Id == user.Id).ToArrayAsync();

            var vm = new KeysViewModel()
            {
                SelectedKey = user.ActiveApiKey,
                Keys = apiKeys
            };

            return View(vm);
        }


        public async Task<IActionResult> Select(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            user.ActiveApiKey = id;

            await _userManager.UpdateAsync(user);

            var apiKeys = await _context.ApiKeys.Where(k => k.User.Id == user.Id).ToArrayAsync();

            var vm = new KeysViewModel()
            {
                SelectedKey = user.ActiveApiKey,
                Keys = apiKeys
            };

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Edit(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            var apiKey = await _context.ApiKeys.SingleOrDefaultAsync(k => k.User.Id == user.Id && k.Id == id);

            return View(apiKey);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, [FromForm]ApiKey entity)
        {
            try
            {
                var existing = await _context.ApiKeys.SingleOrDefaultAsync(k => k.Id == id);

                existing.IsEnabled = entity.IsEnabled;
                existing.Key = entity.Key;
                existing.Description = entity.Description;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}