using AutomacaoFolhaPagamento.Models;
using AutomatizacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace AutomacaoFolhaPagamento.Controllers
{
    [Authorize(Policy = "Logado")]
    public class CargosController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CargosController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ActionResult> Index()
        {
            var cargosLista = await ObterCargos();
            return View(cargosLista);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var cargo = await ObterCargoPorId(id);
            if (cargo == null)
            {
                return NotFound();
            }

            await LoadDepartamentos();
            return View(cargo);
        }

        public async Task<ActionResult> Cadastro()
        {
            await LoadDepartamentos();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CargosViewModel model)
        {
            try
            {
                var client = _clientFactory.CreateClient("CustomSSLValidation");

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
                    return View("Cadastro", new CargosViewModel());
                }

                var userData = new
                {
                    nome_cargo = model.nome_cargo,
                    descricao_cargo = model.descricao_cargo,
                    salario = valorFinal,
                    departamentoId = model.departamentoId
                };

                var content = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Cargos/novoCargo", content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewData["SuccessMessage"] = "Cadastro realizado com sucesso!";
                    await LoadDepartamentos();
                    return View("Cadastro", new CargosViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Cargo já cadastrado.";
                    ModelState.Clear();
                    await LoadDepartamentos();
                    return View("Cadastro", new CargosViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {

                    ViewData["ErrorMessage"] = "Ocorreu um erro ao inserir o cargo. Por favor, tente novamente.";
                    ModelState.Clear();
                    await LoadDepartamentos();
                    return View("Cadastro", new CargosViewModel());
                }

                throw new Exception("Erro no servidor.");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Não foi possível completar o cadastro.";
                ModelState.Clear();
                await LoadDepartamentos();
                return View("Cadastro", new CargosViewModel());
            }

        }

        private async Task<CargoDetalhes> ObterCargoPorId(int id)
        {

            //var client = _clientFactory.CreateClient("");

            //var response = await client.GetAsync($"https://localhost:7067/api/Cargos/retornaCargo/{id}");

            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Cargos/retornaCargo/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var cargoDTOList = JsonSerializer.Deserialize<List<CargoDetalhes>>(jsonString);

                return cargoDTOList.FirstOrDefault(); // usando System.Linq
            }

            return null;
        }

        private async Task<List<ListaCargos>> ObterCargos()
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync("Cargos/retornaCargos");

            var cargosLista = new List<ListaCargos>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var dadosCargos = JsonSerializer.Deserialize<List<CargoDTO>>(jsonString);

                foreach (var item in dadosCargos)
                {
                    var cargo = new ListaCargos
                    {
                        cargo_id = item.id_cargo,
                        nome_cargo = item.nome_cargo,
                        descricao = item.descricao_cargo,
                        salario = item.salario,
                        departamentoId = item.departamentoId
                    };

                    cargosLista.Add(cargo);
                }
            }

            return cargosLista;
        }

        [HttpPost]
        public async Task<ActionResult> SalvarEdicao(CargoDetalhes cargo)

        {
            //var client = _clientFactory.CreateClient("");
            //var response = await client.PutAsync($"https://localhost:7067/api/Cargos/atualizaCargo", content);
            var client = _clientFactory.CreateClient("CustomSSLValidation");

            var cargoEnvio = new CargoDetalhes
            {
                id_cargo = cargo.id_cargo,
                nome_cargo = cargo.nome_cargo,
                descricao_cargo = cargo.descricao_cargo,
                salario = cargo.salario,
                DepartamentoId = cargo.id_departamento
            };

            var content = new StringContent(JsonSerializer.Serialize(cargoEnvio), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("Cargos/atualizaCargo", content);


            if (response.IsSuccessStatusCode)
            {
                ViewData["SuccessMessage"] = "Cadastro atualizado com sucesso!";
                await LoadDepartamentos();
                var cargoAtualizado = await ObterCargoPorId(cargo.id_cargo);
                return View("Detalhes", cargoAtualizado);
            }
            else
            {
                ViewData["ErrorMessage"] = "Ocorreu um erro ao editar o cargo. Por favor, tente novamente.";
                await LoadDepartamentos();
                var cargoAtual = await ObterCargoPorId(cargo.id_cargo);
                return View("Detalhes", cargoAtual);
            }
        }

        private async Task LoadDepartamentos()
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync("Departamentos/listarDepartamentos");

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

        [HttpPost]
        public async Task<IActionResult> DeletaCargo(int id)
        {
            try
            {
                //var client = _clientFactory.CreateClient();
                //var response = await client.DeleteAsync($"https://localhost:7067/api/Cargos/excluir/{id}");

                var client = _clientFactory.CreateClient("CustomSSLValidation");
                var response = await client.DeleteAsync($"Cargos/excluir/{id}");

                if (response.IsSuccessStatusCode)
                {
                    ViewData["SuccessMessage"] = "Cadastro removido com sucesso!";
                    var cargosLista = await ObterCargos();
                    return View("Index", cargosLista);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Cadastro não encontrado!";
                    var cargosLista = await ObterCargos();
                    return View("Index", cargosLista);
                }
            }
            catch (Exception ex)
            {

                ViewData["ErrorMessage"] = "Ocorreu um erro inesperado!";
                var cargosLista = await ObterCargos();
                return View("Index", cargosLista);
            }
        }
    }
}
