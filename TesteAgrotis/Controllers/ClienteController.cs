using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteAgrotis.Models;

namespace TesteAgrotis.Controllers
{
    public class ClienteController : Controller
    {
        private readonly Contexto ctx;
        public ClienteController(Contexto contexto)
        {
            ctx = contexto;
        }

        // GET: ClienteController
        public ActionResult Index()
        {
            var clientes = ctx.Clientes.Where(x => x.Ativo == true).ToList();

            return View(clientes);
        }

        // GET: ClienteController/Create
        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Salvar(Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    return Json(new { success = false, message = "Os dados do cliente estão vazios. Verifique e tente novamente." });
                }
                else if (cliente.Nome == "")
                {
                    return Json(new { success = false, message = "Por favor preencha o campo nome. Verifique e tente novamente." });
                }
                else if (cliente.CEP == "")
                {
                    return Json(new { success = false, message = "Por favor preencha o campo CEP. Verifique e tente novamente." });
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        cliente.DtCreate = DateTime.Now;
                        cliente.Ativo = true;
                        ctx.Clientes.Add(cliente);

                        int clienteSalvo = ctx.SaveChanges();

                        if (clienteSalvo > 0)
                        {
                            return Json(new { success = true, message = "Cliente salvo com sucesso!" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Erro ao salvar o cliente." });
                        }
                    }
                    else
                    {
                        var errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage);
                        return Json(new { success = false, message = "Erro ao validar o cliente.", errors });
                    }
                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Erro ao salvar o cliente: " + ex.Message });
            }

        }

        // GET: ClienteController/Delete/5
        public IActionResult Excluir(int id)

        {
            var cliente = ctx.Clientes.Where(x => x.Id == id && x.Ativo == true).FirstOrDefault();

            if (cliente != null)
            {
                cliente.DtDeleted = DateTime.Now;
                cliente.Ativo = false;

                try
                {
                    ctx.Update(cliente);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return RedirectToAction("Index", "Cliente");
        }

        public IActionResult Detalhes(int id)
        {
            var cliente = ctx.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

    }
}
