using CREFloresEPlantas.Data;
using CREFloresEPlantas.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CREFloresEPlantas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TiposProdutosController : Controller
    {
        private ApplicationDbContext _db;
        public TiposProdutosController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //var data = _db.TiposProdutos.ToList();
            return View(_db.TiposProdutos.ToList());
        }

        //Create - Criar Tipo de Produto
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(TiposProdutos tiposProdutos)
        {
            if(ModelState.IsValid)
            {
                _db.TiposProdutos.Add(tiposProdutos);
                await _db.SaveChangesAsync();
                TempData["save"]="Sucesso!";
                return RedirectToAction(actionName:nameof(Index));
            }
            return View(tiposProdutos);
        }
        //Edit - Editar Tipo de Produto
        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var tipoProduto = _db.TiposProdutos.Find(id);
            if (tipoProduto==null)
            {
                return NotFound();
            }
            return View(tipoProduto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TiposProdutos tiposProdutos)
        {
            if (ModelState.IsValid)
            {
                _db.TiposProdutos.Update(tiposProdutos);
                await _db.SaveChangesAsync();
                TempData["save"] = "Sucesso!";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(tiposProdutos);
        }
        //Details - Detalhes do Tipo de Produto
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tipoProduto = _db.TiposProdutos.Find(id);
            if (tipoProduto == null)
            {
                return NotFound();
            }
            return View(tipoProduto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(TiposProdutos tiposProdutos)
        {
            return RedirectToAction(actionName: nameof(Index));
        }

        //Delete - Apagar Tipo de Produto
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tipoProduto = _db.TiposProdutos.Find(id);
            if (tipoProduto == null)
            {
                return NotFound();
            }
            return View(tipoProduto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, TiposProdutos tiposProdutos)
        {
            if (id==null)
            {
                return NotFound();
            }
            if (id!=tiposProdutos.Id)
            {
                return NotFound();
            }
            var tipoProduto = _db.TiposProdutos.Find(id);

            if (tipoProduto==null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(tipoProduto);
                await _db.SaveChangesAsync();
                TempData["save"] = "Apagado!";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(tiposProdutos);
        }
    }
}
