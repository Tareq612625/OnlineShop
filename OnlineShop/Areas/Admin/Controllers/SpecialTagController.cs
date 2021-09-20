using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{   
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private ApplicationDbContext _db;
        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTags specialTags)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Add(specialTags);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Save Successfully";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(specialTags);
        }
        //*************for updat/edi****************//
        // Get Action Edit Method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTags = _db.SpecialTags.Find(id);
            if (specialTags == null)
            {
                return NotFound();
            }
            return View(specialTags);
        }
        // Post Action Edit Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTags specialTags)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Update(specialTags);
                await _db.SaveChangesAsync();
                TempData["update"] = "Product Update Successfully";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(specialTags);
        }
        //************for Details**********//////
        // Get Action Details Method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        // Post Action Details Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(SpecialTags specialTags)
        {
            return RedirectToAction(actionName: nameof(Index));
        }
        //*************for Delete****************//
        // Get Action Delete Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTags = _db.SpecialTags.Find(id);
            if (specialTags == null)
            {
                return NotFound();
            }
            return View(specialTags);
        }
        // Post Action Edit Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTags specialTags)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != specialTags.Id)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Remove(specialTag);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Product Delete Successfully";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(specialTags);
        }
    }

}
