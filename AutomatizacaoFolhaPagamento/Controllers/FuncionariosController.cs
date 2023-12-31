﻿using AutomacaoFolhaPagamento.Models;
using AutomacaoFolhaPagamento.Models.ApiDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;
using static AutomacaoFolhaPagamento.Models.FuncionarioDTO;

namespace AutomacaoFolhaPagamento.Controllers
{
    [Authorize(Policy = "Logado")]
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
            await LoadUsuarios();
            await LoadCargos();
            return View();
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var funcionario = await ObterFuncionarioPorId(id);

            if (funcionario == null)
            {
                
                return View("Erro");
            }

            await LoadUsuarios();
            await LoadCargos();

            return View(funcionario);
        }

        


        [HttpPost]
        public async Task<IActionResult> AtualizarFuncionario(FuncionarioDTO model)
        {
            List<EnderecoApiDTO> listaEndereco = new List<EnderecoApiDTO>();
            List<TelefoneApiDTO> listaTelefone = new List<TelefoneApiDTO>();

            if(model.enderecos != null)
            {
                model.enderecos.ForEach(end =>
                {
                    EnderecoApiDTO item = new EnderecoApiDTO();
                    item.id = end.id;
                    item.tipo_endereco = end.tipo_endereco;
                    item.rua = end.rua;
                    item.bairro = end.bairro;
                    item.num_endereco = end.num_endereco;
                    item.cep = end.cep;
                    item.cidade = end.cidade;
                    item.uf_estado = end.uf_estado;
                    listaEndereco.Add(item);
                });
            }

            if(model.contatos != null)
            {
                model.contatos.ForEach(cto =>
                {
                    TelefoneApiDTO item = new TelefoneApiDTO();
                    item.id = cto.id;
                    item.tipo_telefone = cto.tipo_telefone;
                    item.numero_contato = cto.numero_contato;
                    listaTelefone.Add(item);
                });
            }

            string usuario = "";

            if (model.funcionario.email_usuario != "SelecionarUsuario")
            {
                usuario = model.funcionario.email_usuario.ToString();
            }

            var apiDto = new FuncionarioApiDTO
            {
               nome = model?.funcionario.nome_funcionario,
               sexo = model?.funcionario.sexo,
               estado_civil = model?.funcionario.estado_civil,
               cargo_id = model?.funcionario.cargo_id,
               data_contratacao = model?.funcionario.data_contratacao,
               cpf = model?.funcionario.cpf,
               email_usuario = usuario,
               enderecos = listaEndereco,
               telefones = listaTelefone
            };

            //var client = _clientFactory.CreateClient();
            

            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var jsonString = JsonSerializer.Serialize(apiDto);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"Funcionarios/atualizaFuncionario/{model.funcionario.id_funcionario}", httpContent);
            //var response = await client.PutAsync($"https://localhost:7067/api/Funcionarios/atualizaFuncionario/{model.funcionario.id_funcionario}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                ViewData["SuccessMessage"] = "Sucesso ao atualizar o funcionário";
                return RedirectToAction("Detalhes", new { id = model.funcionario.id_funcionario });
            }
            else
            {
                ViewData["ErrorMessage"] = "Erro ao atualizar o funcionário";
                return RedirectToAction("Detalhes", new { id = model.funcionario.id_funcionario });
            }
        }

        private async Task<FuncionarioDTO> ObterFuncionarioPorId(int id)
        {
            //var client = _clientFactory.CreateClient();
            //var response = await client.GetAsync($"https://localhost:7067/api/Funcionarios/dadosFuncionarioCompleto/{id}");
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync($"Funcionarios/dadosFuncionarioCompleto/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var funcionarios = JsonSerializer.Deserialize<List<FuncionarioDTO>>(jsonString);
                var funcionarioDTO = funcionarios.FirstOrDefault();

                return funcionarioDTO;
            }

            return null;
        }


        private async Task LoadCargos()
        {
            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync("Cargos/retornaCargos");  

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

        private async Task LoadUsuarios()
        {

            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync("Autenticacao/listarUsuarios");

            //var client = _clientFactory.CreateClient();

            //var response = await client.GetAsync("https://localhost:7067/api/Autenticacao/listarUsuarios");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var usuarios = JsonSerializer.Deserialize<List<UsuarioDTO>>(jsonString);

                ViewData["Usuarios"] = usuarios.Select(d => new SelectListItem
                {
                    Value = d.email.ToString(),
                    Text = $"{d.usuario_id} - {d.email}"
                }).ToList();
            }
        }



        private async Task<List<ListaFuncionarios>> ObterFuncionarios()
        {
            //var client = _clientFactory.CreateClient();
            //var response = await client.GetAsync("https://localhost:7067/api/Funcionarios/dadosFuncionarioCompleto");

            var client = _clientFactory.CreateClient("CustomSSLValidation");
            var response = await client.GetAsync("Funcionarios/dadosFuncionarioCompleto");

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
                    numero_contato = fun.numero_contato,
                    email_usuario = fun.email_usuario
                };


                //var client = _clientFactory.CreateClient();
                //var response = await client.PostAsync("https://localhost:7067/api/Funcionarios/novoFuncionario", content);

                var client = _clientFactory.CreateClient("CustomSSLValidation");
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Funcionarios/novoFuncionario", content);


                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    ViewData["SuccessMessage"] = "Cadastro realizado com sucesso!";
                    await LoadCargos();
                    await LoadUsuarios();
                    ModelState.Clear();
                    return View("Cadastro", new NovoFuncionarioViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Funcionário já cadastrado.";
                    await LoadUsuarios();
                    await LoadCargos();
                    return View("Cadastro", new NovoFuncionarioViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {

                    ViewData["ErrorMessage"] = "Ocorreu um erro ao cadastrar o funcionário. Por favor, tente novamente.";
                    await LoadUsuarios();
                    await LoadCargos();
                    return View("Cadastro", new NovoFuncionarioViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {

                    ViewData["ErrorMessage"] = "Cargo não localizado.";
                    await LoadUsuarios();
                    await LoadCargos();
                    return View("Cadastro", new NovoFuncionarioViewModel());
                }

                throw new Exception("Erro no servidor.");
            }
            catch
            {
                ViewData["ErrorMessage"] = "Não foi possível completar o cadastro.";
                ModelState.Clear();
                await LoadUsuarios();
                await LoadCargos();
                return View("Cadastro", new NovoFuncionarioViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletaFuncionario(int id)
        {
            try
            {
                //var client = _clientFactory.CreateClient();
                //var response = await client.DeleteAsync($"https://localhost:7067/api/Cargos/excluir/{id}");

                var client = _clientFactory.CreateClient("CustomSSLValidation");
                var response = await client.DeleteAsync($"Funcionarios/excluir/{id}");

                if (response.IsSuccessStatusCode)
                {
                    ViewData["SuccessMessage"] = "Cadastro removido com sucesso!";
                    var funcionariosLista = await ObterFuncionarios();
                    return View("Index", funcionariosLista);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Cadastro não encontrado!";
                    var funcionariosLista = await ObterFuncionarios();
                    return View("Index", funcionariosLista);
                }
            }
            catch (Exception ex)
            {

                ViewData["ErrorMessage"] = "Ocorreu um erro inesperado!";
                var funcionariosLista = await ObterFuncionarios();
                return View("Index", funcionariosLista);
            }
        }
    }
}
