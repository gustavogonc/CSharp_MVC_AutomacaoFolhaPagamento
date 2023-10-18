using AutomacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public FuncionariosController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var funcionarios = await ObterFuncionarios();
            return View(funcionarios);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro()
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



        private async Task<List<ListaFuncionarios>> ObterFuncionarios()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7067/api/Funcionarios/dadosFuncionarioCompleto");

            var funcionariosLista = new List<ListaFuncionarios>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var dadosFuncionarios = JsonSerializer.Deserialize<List<FuncionarioDTO>>(jsonString);

                foreach (var item in dadosFuncionarios)
                {
                    var funcionario = new ListaFuncionarios
                    {
                        funcionario_id = item.funcionario.id_funcionario,
                        nome_completo = item.funcionario.nome_funcionario,
                        deparamento = item.funcionario.departamento
                    };

                    funcionariosLista.Add(funcionario);
                }
            }

            return funcionariosLista;
        }



        [HttpPost]
        public async Task<IActionResult> Cadastro(NovoFuncionarioViewModel fun)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var data = new
                {
                    nome = fun.nome,
                    cpf = fun.cpf,
                    sexo = fun.sexo ,
                    cargo_id = fun.cargo_id,
                    data_contratacao = fun.data_contratacao, 
                    estado_civil = fun.estado_civil,
                    rua = fun.rua,
                    tipo_endereco = fun.tipo_endereco, 
                    num_endereco =  fun.num_endereco,
                    bairro = fun.bairro,
                    cep = fun.cep,
                    cidade = fun.cidade,
                    uf_estado = fun.uf_estado,
                    tipo_telefone = fun.tipo_telefone,
                    numero_contato = fun.numero_contato
                };

                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7067/api/Funcionarios/novoFuncionario", content);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    ViewData["SuccessMessage"] = "Cadastro realizado com sucesso!";
                    await LoadCargos();
                    ModelState.Clear();
                    return View("Cadastro", new NovoFuncionarioViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Funcionário já cadastrado.";
                    ModelState.Clear();
                    await LoadCargos();
                    return View("Cadastro", new NovoFuncionarioViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {

                    ViewData["ErrorMessage"] = "Ocorreu um erro ao cadastrar o funcionário. Por favor, tente novamente.";
                    ModelState.Clear();
                    await LoadCargos();
                    return View("Cadastro", new NovoFuncionarioViewModel());
                }

                throw new Exception("Erro no servidor.");
            }
            catch
            {
                ViewData["ErrorMessage"] = "Não foi possível completar o cadastro.";
                ModelState.Clear();
                await LoadCargos();
                return View("Cadastro", new NovoFuncionarioViewModel());
            }
        }

    }
}
