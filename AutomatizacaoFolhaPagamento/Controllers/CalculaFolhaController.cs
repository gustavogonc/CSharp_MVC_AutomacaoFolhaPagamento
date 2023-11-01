using AutomacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static AutomacaoFolhaPagamento.Models.FuncionarioDTO;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class CalculaFolhaController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public CalculaFolhaController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var funcionarios = await ObterTodosFuncionarios();

            var model = new CalculaFolhaViewModel
            {
                Funcionarios = funcionarios,
                DetalhesFuncionario = null
            };

            return View(model);
        }

        public async Task<List<FuncionariosCalculo>> ObterTodosFuncionarios()
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Funcionarios");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<FuncionariosCalculo>>(jsonString);
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarDetalhesFuncionario(int funcionarioId)
        {
            var detalhesFuncionario = await ObterDetalhesFuncionarioPorId(funcionarioId);

            var funcionarios = await ObterTodosFuncionarios();

            var model = new CalculaFolhaViewModel
            {
                Funcionarios = funcionarios,
                DetalhesFuncionario = detalhesFuncionario,
                SelecionadoFuncionarioId = funcionarioId
            };

            return View("Index", model);
        }


        public async Task<List<FuncionarioDetalhes>> ObterDetalhesFuncionarioPorId(int id)
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Funcionarios/dadosFuncionarioCalculo/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<FuncionarioDetalhes>>(jsonString);
            }

            return null;
        }
    }
}
