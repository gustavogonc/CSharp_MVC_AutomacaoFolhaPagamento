using Microsoft.AspNetCore.Mvc;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class CadastroController : Controller
    {

        public IActionResult CadastroUsuario()
        {
            return View();
        }
    }
}
