using Microsoft.AspNetCore.Mvc;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class FuncionarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
