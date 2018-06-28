using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Swampnet.Evl.Web.Controllers
{
    public class ReferenceCrudController : Controller
    {
        // GET: ReferenceCrud
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReferenceCrud/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReferenceCrud/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReferenceCrud/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReferenceCrud/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReferenceCrud/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReferenceCrud/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReferenceCrud/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}