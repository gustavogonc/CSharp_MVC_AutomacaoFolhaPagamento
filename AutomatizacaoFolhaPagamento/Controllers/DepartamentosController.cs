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
        // GET: DepartamentosController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DepartamentosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartamentosController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Cadastro(DepartamentosViewModel dep)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var data = new
                {
                    nome_departamento = dep.NomeDepartamento,
                    descricao_departamento = dep.DescricaoDepartamento
                };

                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7067/api/Departamentos/novoDepartamento", content);
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
                    
                    ViewData["ErrorMessage"] = "Ocorreu um erro ao registrar o usuário. Por favor, tente novamente.";
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
