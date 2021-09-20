using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Customer.Controllers
{   [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db) {
            _db = db;
        }
        //Get Checkout action method
        
        public IActionResult CheckOut()
        {
            return View();
        }
        //Post checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(Order anOrder) {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null) {
                foreach (var product in products) { 
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }
            anOrder.OrderNo = GetOrderNo();
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products",new List<Products>());


            return View();
        }

        //OrderNo Generate Method
        public String GetOrderNo() {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("ON-00");
        }
    }
}
