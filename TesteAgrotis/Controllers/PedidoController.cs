using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TesteAgrotis.Models;

namespace TesteAgrotis.Controllers
{
    public class PedidoController : Controller
    {
        private readonly Contexto ctx;

        public PedidoController(Contexto contexto)
        {
            ctx = contexto;
        }

        public IActionResult Index()
        {
            var pedidos = ctx.Pedido.Include(pedido => pedido.Cliente).Where(x => x.Ativo == true).ToList();

            return View(pedidos);
        }

        public IActionResult Novo()
        {
            ViewBag.LstClientes = ctx.Clientes.Where(x => x.Ativo == true).Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Nome }).ToList();
            ViewBag.LstProdutos = ctx.Produto.Where(x => x.Ativo == true).ToList();

            return View();
        }

        public IActionResult Detalhes(int id)
        {
            var pedido = ctx.Pedido
                            .Include(p => p.Cliente)
                            .Include(p => p.Itens)
                                .ThenInclude(item => item.Produto)
                            .FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        public IActionResult Excluir(int id)
        {
            var pedido = ctx.Pedido.Include(p => p.Itens).FirstOrDefault(p => p.Id == id);
            
            if (pedido == null)
            {
                return NotFound();
            }

            foreach (var item in pedido.Itens)
            {
                item.Ativo = true;
                item.DtDeleted = DateTime.Now;
                ctx.PedidoItem.Update(item);
            }

            pedido.Ativo = false;
            pedido.DtDeleted = DateTime.Now;
            ctx.Pedido.Update(pedido);

            ctx.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public JsonResult SalvarPedido(Pedido pedido, List<PedidoItem> produtos)
        {
            using (var transaction = ctx.Database.BeginTransaction())
            {
                try
                {
                    if (produtos == null || produtos.Count == 0)
                    {
                        return Json(new { success = false, message = "A tabela de produtos está vazia. Por favor, adicione produtos." });
                    }
                    else if (pedido == null)
                    {
                        return Json(new { success = false, message = "Os dados do Produtos estão vazios, verifique e tente novamente." });
                    }
                    else
                    {
                        pedido.Ativo = true;
                        pedido.DtCreate = DateTime.Now;
                        pedido.DataEmissao = DateTime.Now;
                        ctx.Pedido.Add(pedido);

                        int pedidoSalvo = ctx.SaveChanges();

                        if (pedidoSalvo > 0)
                        {
                            foreach (var produto in produtos)
                            {
                                produto.PedidoId = pedido.Id;
                                produto.Ativo = true;
                                produto.DtCreate = DateTime.Now;
                                ctx.PedidoItem.Add(produto);
                            }
                            ctx.SaveChanges();

                            transaction.Commit();

                            return Json(new { success = true, message = "Pedido salvo com sucesso!" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Erro ao salvar o pedido." });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = "Erro ao salvar o pedido: " + ex.Message });
                }
            }
        }


    }
}
