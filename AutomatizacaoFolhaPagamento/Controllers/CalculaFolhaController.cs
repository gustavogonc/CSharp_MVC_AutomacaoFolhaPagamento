using AutomacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using static AutomacaoFolhaPagamento.Models.FuncionarioDTO;

namespace AutomacaoFolhaPagamento.Controllers
{
    [Authorize(Policy = "Logado")]
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

        public decimal CorrigeValorDecimal(decimal valorOriginal)
        {
         
            string valorString = valorOriginal.ToString();

            
            if (valorString.Length > 2)
            {

                valorString = valorString.Insert(valorString.Length - 2, ".");
            }
            else
            {
                valorString = valorString.PadLeft(3, '0').Insert(1, ".");
            }

            return Convert.ToDecimal(valorString, CultureInfo.InvariantCulture);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarValores(ProventosViewModel model)
        {
            List<ProventosListModel> proventos = new List<ProventosListModel>();

            model.Proventos.ForEach(p => {
                var item = new ProventosListModel();
                item.id_funcionario = model.FuncionarioId;
                item.mes = model.Mes;
                item.ano = model.Ano;
                if (p.Valor.HasValue)
                {
                    item.valor = CorrigeValorDecimal(p.Valor.Value);
                }
                item.nome_valor = p.NomeValor;
                item.tipo_valor = p.TipoValor;
                item.data = DateTime.Now;
                proventos.Add(item);
            });

            var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://localhost:7067/api/Calculo/AdicionaValores", proventos);
            //var client = _clientFactory.CreateClient("CustomSSLValidation");
            //var response = await client.PostAsJsonAsync("Calculo/AdicionaValores", proventos);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                ViewData["ErrorMessage"] = $"Já existe cálculo para esse funcionário referente a {model.Mes}/{model.Ano}.";
                ModelState.AddModelError("", "Ocorreu um erro ao adicionar valores.");
                return UnprocessableEntity();
            }
            else
            {
                ViewData["ErrorMessage"] = $"Erro inesperado.";
                ModelState.AddModelError("", "Ocorreu um erro ao adicionar valores.");
                return BadRequest();
            }
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
