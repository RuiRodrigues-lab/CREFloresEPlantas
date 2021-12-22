using CREFloresEPlantas.Data;
using CREFloresEPlantas.Models;
using CREFloresEPlantas.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CREFloresEPlantas.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Produtos> produtos = HttpContext.Session.Get<List<Produtos>>("produtos");
            if (produtos != null)
            {
                foreach (var produto in produtos)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.PorductId = produto.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }

            anOrder.OrderNo = GetOrderNo();
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("produtos", new List<Produtos>());
            return View();
        }


        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

    }
}
