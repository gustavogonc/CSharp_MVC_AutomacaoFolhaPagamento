using AutomacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using AutomatizacaoFolhaPagamento.Models;

namespace AutomacaoFolhaPagamento.Controllers
{
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
                         data_cricacao = item.data_criacao
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
                    return View("Index", new DepartamentosViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Departamento já cadastrado.";
                    ModelState.Clear();
                    return View("Index", new DepartamentosViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    
                    ViewData["ErrorMessage"] = "Ocorreu um erro ao cadastrar o departamento. Por favor, tente novamente.";
                    ModelState.Clear();
                    return View("Index", new DepartamentosViewModel());
                }

                throw new Exception("Erro no servidor."); 
            }
            catch
            {
                ViewData["ErrorMessage"] = "Não foi possível completar o cadastro.";
                ModelState.Clear();
                return View("Index", new DepartamentosViewModel());
            }
        }

        // GET: DepartamentosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DepartamentosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartamentosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DepartamentosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
