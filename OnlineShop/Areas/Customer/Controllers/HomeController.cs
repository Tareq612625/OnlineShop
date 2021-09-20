using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Controllers;
using OnlineShop.Utility;
using X.PagedList;


namespace OnlineShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        //application db context
        private ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? page)
        {
            var Products = _db.Products.Include(c => c.ProductTypes).Include(d => d.SpecialTags).ToList().ToPagedList(pageNumber:page??1,pageSize:12);
            return View(Products); 

        }
        public IActionResult Privacy()
        {
            return View();
        }
          
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if (product == null) {
                return NotFound();
            }
            return View(product);
        }
        //Post Product Details Action Method
        [HttpPost]
        [ActionName("Details")]
        public ActionResult ProductDetails(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null) {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return View(product);
        }
        // Get Remove for session
        [HttpPost]
        public IActionResult Remove(int? id) {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null) {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null) {
                    products.Remove(product);
                    TempData["RemoveCard"] = "Remove From Card Successfully";
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // Get Remove from cart
        [ActionName("Remove")]
        public IActionResult RemoveCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    TempData["RemoveCard"] = "Remove From Card Successfully";
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        //GET Product Cart Action Method
        public IActionResult Cart() {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null) {
                products = new List<Products>();
            }
            return View(products);
        }

    }
}
