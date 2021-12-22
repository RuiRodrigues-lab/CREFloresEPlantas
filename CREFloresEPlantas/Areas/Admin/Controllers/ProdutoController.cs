using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CREFloresEPlantas.Data;
using CREFloresEPlantas.Models;


namespace CREFloresEPlantas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProdutoController : Controller
    {
        private ApplicationDbContext _db;
        //serve para guardar imagens no root > images, se houver tempo meto em blobstorage
        private IHostingEnvironment _he;
        public ProdutoController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Produtos.Include(c=>c.TiposProdutos).Include(f=>f.SpecialTag).ToList());
        }
        [HttpPost]
        public IActionResult Index(double? baixoP, double? altoP)
        {
            var produtos = _db.Produtos.Include(c => c.TiposProdutos).Include(c => c.SpecialTag).Where(c=>c.Preco>=baixoP&&c.Preco<=altoP).ToList();
            if (baixoP==null || altoP==null)
            {
                produtos = _db.Produtos.Include(c => c.TiposProdutos).Include(c => c.SpecialTag).ToList();
            }
            return View(produtos);
        }
        //Create - Criar Produto
        public IActionResult Create()
        {
            ViewData["tipoProdutoId"] = new SelectList(_db.TiposProdutos.ToList(), "Id", "TipoProduto");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Nome");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Produtos produto,IFormFile imagem)
        {
            if (ModelState.IsValid)
            {
                var procurarProduto = _db.Produtos.FirstOrDefault(c => c.Nome == produto.Nome);
                if (procurarProduto != null)
                {
                    ViewBag.message = "Este produto já existe.";
                    ViewData["tipoProdutoId"] = new SelectList(_db.TiposProdutos.ToList(), "Id", "TipoProduto");
                    ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Nome");
                    return View(produto);
                }

                if (imagem!=null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(imagem.FileName));
                    await imagem.CopyToAsync(new FileStream(name, FileMode.Create));
                    produto.Imagem = "Images/" + imagem.FileName;
                }
                if (imagem==null)
                {
                    produto.Imagem = "Images/SemImagem.png";
                }
                _db.Produtos.Add(produto);
                await _db.SaveChangesAsync();
                TempData["save"] = "Sucesso!";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(produto);
        }
        //Edit - Editar Produto
        public ActionResult Edit(int? id)
        {
            ViewData["tipoProdutoId"] = new SelectList(_db.TiposProdutos.ToList(), "Id", "TipoProduto");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Nome");
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Produtos.Include(c => c.TiposProdutos).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Produtos produto, IFormFile imagem)
        {
            if (ModelState.IsValid)
            {
                if (imagem != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(imagem.FileName));
                    await imagem.CopyToAsync(new FileStream(name, FileMode.Create));
                    produto.Imagem = "Images/" + imagem.FileName;
                }
                if (imagem == null)
                {
                    produto.Imagem = "Images/SemImagem.png";
                }
                _db.Produtos.Update(produto);
                await _db.SaveChangesAsync();
                TempData["save"] = "Sucesso!";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(produto);
        }

        //Details - Detalhes Produto
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Produtos.Include(c => c.TiposProdutos).Include(c => c.SpecialTag).FirstOrDefault(c=>c.Id==id);
            if (product==null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Delete - Apagar produtos
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var produto = _db.Produtos.Include(c=>c.SpecialTag).Include(c => c.TiposProdutos).Where(c => c.Id == id).FirstOrDefault();
            if(produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var produto = _db.Produtos.FirstOrDefault(c => c.Id == id);
            if (produto == null)
            {
                return NotFound();
            }
            _db.Produtos.Remove(produto);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
