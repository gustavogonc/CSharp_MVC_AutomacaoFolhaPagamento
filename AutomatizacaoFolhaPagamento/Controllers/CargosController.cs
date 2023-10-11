using AutomacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
