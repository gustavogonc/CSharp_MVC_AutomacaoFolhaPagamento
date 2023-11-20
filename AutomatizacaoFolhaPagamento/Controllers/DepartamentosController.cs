using AutomacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using AutomatizacaoFolhaPagamento.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace AutomacaoFolhaPagamento.Controllers
{
    [Authorize(Policy = "Logado")]
    public class DepartamentosController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public DepartamentosController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ActionResult> Index()
        {
            var departamentosLista = await ObterDepartamentos();
            return View(departamentosLista);
        }


        public ActionResult Cadastro()
        {
            return View();
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var departamento = await ObterDepartamentoPorId(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }

        private async Task<ListaDepartamentos> ObterDepartamentoPorId(int id)
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Departamentos/departamentoId/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var departamentoDTO = JsonSerializer.Deserialize<DepartamentoDTO>(jsonString);

                return new ListaDepartamentos
                {
                    id_departamento = departamentoDTO.id_departamento,
                    nome_departamento = departamentoDTO.nome_departamento,
                    descricao_departamento = departamentoDTO.descricao_departamento,
                    data_criacao = departamentoDTO.data_criacao
                };
            }

            return null; // ou você pode lançar uma exceção aqui dependendo da sua preferência
        }

        private async Task<List<ListaDepartamentos>> ObterDepartamentos()
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync("Departamentos/listarDepartamentos");

            var departamentosLista = new List<ListaDepartamentos>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var dadosDepartamento = JsonSerializer.Deserialize<List<DepartamentoDTO>>(jsonString);

                foreach (var item in dadosDepartamento)
                {
                    var departamento = new ListaDepartamentos
                    {
                         id_departamento= item.id_departamento,
                         nome_departamento = item.nome_departamento,
                         descricao_departamento = item.descricao_departamento,
                         data_criacao = item.data_criacao
                    };

                    departamentosLista.Add(departamento);
                }
            }

            return departamentosLista;
        }

        [HttpPost]
        public async Task<ActionResult> Cadastro(DepartamentosViewModel dep)
        {
            try
            {
                var client = _clientFactory.CreateClient("CustomSSLValidation");

                var data = new
                {
                    nome_departamento = dep.NomeDepartamento,
                    descricao_departamento = dep.DescricaoDepartamento
                };

                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Departamentos/novoDepartamento", content);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    ViewData["SuccessMessage"] = "Cadastro realizado com sucesso!";
                    var info = await ObterDepartamentos();
                    return View("Cadastro", new DepartamentosViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Departamento já cadastrado.";
                    ModelState.Clear();
                    var info = await ObterDepartamentos();
                    return View("Cadastro", new DepartamentosViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    
                    ViewData["ErrorMessage"] = "Ocorreu um erro ao cadastrar o departamento. Por favor, tente novamente.";
                    ModelState.Clear();
                    var info = await ObterDepartamentos();
                    return View("Cadastro", new DepartamentosViewModel());
                }

                throw new Exception("Erro no servidor."); 
            }
            catch
            {
                ViewData["ErrorMessage"] = "Não foi possível completar o cadastro.";
                ModelState.Clear();
                var info = await ObterDepartamentos();
                return View("Cadastro", new DepartamentosViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> SalvarEdicao(ListaDepartamentos departamentoEditado)
        {
            try
            {
                var client = _clientFactory.CreateClient("CustomSSLValidation");

                var data = new
                {
                    id_departamento = departamentoEditado.id_departamento,
                    nome_departamento = departamentoEditado.nome_departamento,
                    descricao_departamento = departamentoEditado.descricao_departamento
                };

                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");


                var response = await client.PostAsync($"Departamentos/atualizaDepartamento", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["SuccessMessage"] = "Cadastro atualizado com sucesso!";
                    return RedirectToAction("Detalhes", new { id = departamentoEditado.id_departamento });
                }
                else
                {
                    ViewData["ErrorMessage"] = "Ocorreu um erro ao atualizar o departamento.";
                    return View("Detalhes", departamentoEditado);
                }
            }
            catch
            {
                ViewData["ErrorMessage"] = "Não foi possível completar a atualização.";
                return View("Detalhes", departamentoEditado);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletaDepartamento(int id)
        {
            try
            {
                //var client = _clientFactory.CreateClient();
                //var response = await client.DeleteAsync($"https://localhost:7067/api/Departamentos/excluir/{id}");

                var client = _clientFactory.CreateClient("CustomSSLValidation");
                var response = await client.DeleteAsync($"Departamentos/excluir/{id}");

                if (response.IsSuccessStatusCode)
                {
                    ViewData["SuccessMessage"] = "Cadastro removido com sucesso!";
                    var departamentosLista = await ObterDepartamentos();
                    return View("Index", departamentosLista);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Cadastro não encontrado!";
                    var departamentosLista = await ObterDepartamentos();
                    return View("Index", departamentosLista);
                }
            }
            catch (Exception ex)
            {

                ViewData["ErrorMessage"] = "Ocorreu um erro inesperado!";
                var departamentosLista = await ObterDepartamentos();
                return View("Index", departamentosLista);
            }
        }
    }
}
