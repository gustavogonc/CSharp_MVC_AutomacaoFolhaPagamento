using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomacaoFolhaPagamento.Controllers
{
    [Authorize(Policy = "Logado")]
    public class CadastroController : Controller
    {

        public IActionResult CadastroUsuario()
        {
            return View();
        }
    }
}
