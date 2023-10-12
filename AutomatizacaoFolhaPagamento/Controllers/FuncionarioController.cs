using AutomacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public FuncionarioController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            await LoadCargos();
            return View();
        }

        private async Task LoadCargos()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7067/api/Cargos/retornaCargos");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var cargos = JsonSerializer.Deserialize<List<CargosDTO>>(jsonString);

                ViewData["Cargos"] = cargos.Select(d => new SelectListItem
                {
                    Value = d.id_cargo.ToString(),
                    Text = $"{d.id_cargo} - {d.nome_cargo}"
                }).ToList();
            }
        }

    }
}
