using CREFloresEPlantas.Data;
using CREFloresEPlantas.Models;
using CREFloresEPlantas.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CREFloresEPlantas.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Produtos.Include(c=>c.TiposProdutos).Include(c=>c.SpecialTag).ToList());
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

        //Details - Detalhes do produto versão cliente
        public ActionResult Detail(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var produto = _db.Produtos.Include(c => c.TiposProdutos).FirstOrDefault(c => c.Id == id);
            if (produto==null)
            {
                return NotFound();
            }
            return View(produto);
        }
        [HttpPost]
        [ActionName("Detail")]
        public ActionResult ProductDetail(int? id)
        {
            List<Produtos> produtos = new List<Produtos>();
            if (id == null)
            {
                return NotFound();
            }
            var produto = _db.Produtos.Include(c => c.TiposProdutos).FirstOrDefault(c => c.Id == id);
            if (produto == null)
            {
                return NotFound();
            }
            //fica em cache na sessão
            produtos = HttpContext.Session.Get<List<Produtos>>("produtos");
            if (produtos == null)
            {
                produtos = new List<Produtos>();
            }
            produtos.Add(produto);
            HttpContext.Session.Set("produtos", produtos);
            //return View(produto);
            return RedirectToAction(nameof(Index));
        }

        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Produtos> produtos = HttpContext.Session.Get<List<Produtos>>("produtos");
            if (produtos != null)
            {
                var produto = produtos.FirstOrDefault(c => c.Id == id);
                if (produto != null)
                {
                    produtos.Remove(produto);
                    HttpContext.Session.Set("produtos", produtos);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Produtos> produtos = HttpContext.Session.Get<List<Produtos>>("produtos");
            if (produtos != null)
            {
                var produto = produtos.FirstOrDefault(c => c.Id == id);
                if (produto!= null)
                {
                    produtos.Remove(produto);
                    HttpContext.Session.Set("produtos", produtos);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart()
        {
            List<Produtos> produtos = HttpContext.Session.Get<List<Produtos>>("produtos");
            if (produtos == null)
            {
                produtos = new List<Produtos>();
            }
            return View(produtos);
        }


    }
}
