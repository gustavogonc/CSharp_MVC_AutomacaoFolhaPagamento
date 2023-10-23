using AutomatizacaoFolhaPagamento.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using AutomacaoFolhaPagamento.Models;
using System.Text;

namespace AutomatizacaoFolhaPagamento.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
                    HttpClientHandler clientHandler = new HttpClientHandler();
        public LoginController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            
            try
            {
                var client = _clientFactory.CreateClient("CustomSSLValidation");

                var loginData = new
                {
                    email = model.Email,
                    senha = model.Password
                };

                var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Autenticacao/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(body);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginResponse.usuario.email),
                        new Claim(ClaimTypes.Role, loginResponse.usuario.administrador == 1 ? "Admin" : "User")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Usuário ou senha inválidos.";
                }
                else
                {
                    ViewData["ErrorMessage"] = "Ocorreu um erro ao tentar fazer login. Por favor, tente novamente.";
                }

                return View("Login");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}
