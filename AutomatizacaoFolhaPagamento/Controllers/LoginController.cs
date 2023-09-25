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

namespace AutomatizacaoFolhaPagamento.Controllers
{
    public class LoginController : Controller
    {
        

        public LoginController()
        {
        }

        /*[HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }*/

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7067/api/Autenticacao/login?="),
                Headers =
            {
                { "User-Agent", "insomnia/2023.5.8" },
            },
                Content = new StringContent($"{{\n\t \"email\": \"{model.Email}\",\n  \"senha\": \"{model.Password}\"\n}}")

                {
                    Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(body);

                // Criação de uma identidade e assinatura para o usuário.
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginResponse.usuario.email),
                        new Claim(ClaimTypes.Role, loginResponse.usuario.administrador == 1 ? "Admin" : "User")
                    };

                var claimsIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
               return RedirectToAction("Login");
                   
            }

        }
    }
}
