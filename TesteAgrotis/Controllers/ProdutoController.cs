using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteAgrotis.Models;

namespace TesteAgrotis.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly Contexto ctx;
        public ProdutoController(Contexto contexto)
        {
            ctx = contexto;
        }

        // GET: ProdutoController
        public ActionResult Index()
        {
            List<Produto> produtos = ctx.Produto.Where(x => x.Ativo == true).ToList();
            return View(produtos);
        }

        // GET: ProdutoController/Create
        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Salvar(Produto produto)
        {
            using (var transaction = ctx.Database.BeginTransaction())
            {
                try
                {
                    if (produto == null)
                    {
                        return Json(new { success = false, message = "Os dados do produto estão vazios. Verifique e tente novamente." });
                    }
                    else if (produto.PrecoUnitario == 0)
                    {
                        return Json(new { success = false, message = "Por favor, preencha o campo Valor Unitário. Verifique e tente novamente." });
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            produto.DtCreate = DateTime.Now;
                            produto.Ativo = true;
                            ctx.Produto.Add(produto);

                            int produtoSalvo = ctx.SaveChanges();

                            if (produtoSalvo > 0)
                            {
                                transaction.Commit();
                                return Json(new { success = true, message = "Produto salvo com sucesso!" });
                            }
                            else
                            {
                                return Json(new { success = false, message = "Erro ao salvar o produto." });
                            }
                        }
                        else
                        {
                            var errors = ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage);
                            return Json(new { success = false, message = "Erro ao validar o produto.", errors });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = "Erro ao salvar o produto: " + ex.Message });
                }
            }
        }

        public IActionResult Excluir(int id)
        {
            var produto = ctx.Produto.Find(id);
            if (produto == null)
            {
                return NotFound();
            }

            produto.Ativo = false;
            produto.DtDeleted = DateTime.Now;

            ctx.Produto.Update(produto);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Detalhes(int id)
        {
            var produto = ctx.Produto.FirstOrDefault(p => p.Id == id && p.Ativo);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

    }
}
