using AutomatizacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class CadastroUsuarioController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CadastroUsuarioController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(CadastroViewModel form)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var userData = new
                {
                    nome = form.Nome,
                    email = form.Email,
                    senha = form.Senha,
                    administrador = form.Tipo
                };

                var content = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7067/api/Autenticacao/registrar", content);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    ViewData["SuccessMessage"] = "Cadastro realizado com sucesso!";
                    return View("Cadastro", new CadastroViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Usuário já cadastrado.";
                    ModelState.Clear();
                    return View("Cadastro", new CadastroViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    // Você pode adicionar uma mensagem mais específica para diferentes códigos de erro se necessário
                    ViewData["ErrorMessage"] = "Ocorreu um erro ao registrar o usuário. Por favor, tente novamente.";
                    ModelState.Clear();
                    return View("Cadastro", new CadastroViewModel());
                }

                throw new Exception("Erro no servidor."); // Se a resposta não foi bem-sucedida
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Não foi possível completar o cadastro.";
                ModelState.Clear();
                return View("Cadastro", new CadastroViewModel());
            }
        }
    }
}
