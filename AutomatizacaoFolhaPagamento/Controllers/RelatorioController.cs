
using Microsoft.AspNetCore.Mvc;
using AutomacaoFolhaPagamento.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace AutomacaoFolhaPagamento.Controllers
{
    [Authorize(Policy = "Logado")]
    public class RelatorioController : Controller
    {
 
        private readonly IHttpClientFactory _clientFactory;
        public RelatorioController(IHttpClientFactory clientFactory)
        {
       
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Detalhado()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Relatorio(int MesSelecionado)
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Relatorio/listaRelatorio/{MesSelecionado}");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var objetoRetorno = JsonSerializer.Deserialize<List<HistPagamentoModel>>(body);
                return View("Detalhado", objetoRetorno);
            }
            else
            {
                return RedirectToAction("Erro");
            }


        }

        public async Task<FuncionarioViewModel> ReturnFuncionarios()
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Relatorio/listaFuncionario");

            FuncionarioViewModel func = new FuncionarioViewModel();

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var objetoRetorno = JsonSerializer.Deserialize<List<Funcionario>>(body);

                var viewModel = new FuncionarioViewModel
                {
                    funcionarioBasicoList = objetoRetorno
                };

                func = viewModel;
            }
            
            return func;
        }

        public async Task<IActionResult> Funcionario()
        {
            var dados = await ReturnFuncionarios();
            return View("Funcionario", dados);
        }

        [HttpPost]
        public async Task<IActionResult> Funcionario(string Funcionario, string MesSelecionado)
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Relatorio/funcionarioDeducoes/{Funcionario}/{MesSelecionado}");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var objetoRetorno = JsonSerializer.Deserialize<FuncionarioViewModel>(body);
                return View("Funcionario", objetoRetorno);
            }
            else
            {
                return View("Funcionario", new FuncionarioViewModel());
            }

        }


        public async Task<IActionResult> Dashboard()
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Relatorio/dashboard");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var objetoRetorno = JsonSerializer.Deserialize<GraficosViewModel>(body);
                return View(objetoRetorno);
            }
            else
            {
                return RedirectToAction("Erro");
            }
       
        }



    }

}
