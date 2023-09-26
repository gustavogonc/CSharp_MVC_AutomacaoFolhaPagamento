using Microsoft.AspNetCore.Mvc;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class CadastroUsuarioController : Controller
    {

        public IActionResult Cadastro()
        {
            return View();
        }
    }
}