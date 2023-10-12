using AutomacaoFolhaPagamento.Models;
using AutomatizacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class CargosController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CargosController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        // GET: CargosController
        public async Task<ActionResult> Index()
        {
            await LoadDepartamentos();
            return View();
        }

        // GET: CargosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CargosController/Create
        public async Task<IActionResult> Create()
        {
           

            return View();
        }

        // POST: CargosController/Create
        [HttpPost]
        public async Task<IActionResult> Create(CargosViewModel model)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                string salarioString = model.salario;
                salarioString = salarioString.Replace("R$", "").Trim();
                float salarioFloat;

                float valorFinal;

                if (float.TryParse(salarioString, NumberStyles.Float, CultureInfo.InvariantCulture, out salarioFloat))
                {
                    valorFinal = salarioFloat;
                }
                else
                {
                    ViewData["ErrorMessage"] = "Formato de moeda não suportado.";
                    await LoadDepartamentos();
                    return View("Index", new CargosViewModel());
                }

                var userData = new
                {
                    nome_cargo = model.nome_cargo,
                    descricao_cargo = model.descricao_cargo,
                    salario = valorFinal,
                    departamentoId = model.departamentoId
                };

                var content = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7067/api/Cargos/novoCargo", content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewData["SuccessMessage"] = "Cadastro realizado com sucesso!";
                    await LoadDepartamentos();
                    return View("Index", new CargosViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Cargo já cadastrado.";
                    ModelState.Clear();
                    await LoadDepartamentos();
                    return View("Index", new CargosViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {

                    ViewData["ErrorMessage"] = "Ocorreu um erro ao inserir o cargo. Por favor, tente novamente.";
                    ModelState.Clear();
                    await LoadDepartamentos();
                    return View("Index", new CargosViewModel());
                }

                throw new Exception("Erro no servidor.");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Não foi possível completar o cadastro.";
                ModelState.Clear();
                await LoadDepartamentos();
                return View("Index", new CargosViewModel());
            }
        }

        // GET: CargosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CargosController/Edit/5
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

        // GET: CargosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        private async Task LoadDepartamentos()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7067/api/Departamentos/listarDepartamentos");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var departamentos = JsonSerializer.Deserialize<List<DepartamentoDTO>>(jsonString);

                ViewData["Departamentos"] = departamentos.Select(d => new SelectListItem
                {
                    Value = d.id_departamento.ToString(),
                    Text = $"{d.id_departamento} - {d.nome_departamento}"
                }).ToList();
            }
        }
        // POST: CargosController/Delete/5
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
