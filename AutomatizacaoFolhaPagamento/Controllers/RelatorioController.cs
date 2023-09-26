using AutomacaoFolhaPagamento.Repository;
using Microsoft.AspNetCore.Mvc;
using AutomacaoFolhaPagamento.Models;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly ObterPagamentoRepository _pagamentoRepository;
        private readonly FuncionarioDeducoesRepository _funcionarioDeducoesRepository;
        public RelatorioController()
        {
            _pagamentoRepository = new ObterPagamentoRepository();
            _funcionarioDeducoesRepository = new FuncionarioDeducoesRepository();   
        }

        public IActionResult Detalhado()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Relatorio(int MesSelecionado)
        {
           
            var pagamentosFiltrados = _pagamentoRepository.ObterPagamentosFiltrados(MesSelecionado);  
            return View("Detalhado", pagamentosFiltrados);  
        }
        public IActionResult Funcionario()
        {
         
            var funcionarioBasicoList = _funcionarioDeducoesRepository.ObterFuncionariosBasicos();
            var viewModel = new FuncionarioViewModel
            {
                FuncionarioBasicoList = funcionarioBasicoList
            };

            return View("Funcionario", viewModel);
        }
        [HttpPost]
        public IActionResult Funcionario(string Funcionario)
        {
            var funcionarioBasicoList = _funcionarioDeducoesRepository.ObterFuncionariosBasicos();

            var funcionarioDeducoesList = _funcionarioDeducoesRepository.ObterFuncionarioDeducoes();
            var funcionarioDeducoesFiltrados = funcionarioDeducoesList.Where(f => f.CPF == Funcionario).ToList();
            var viewModel = new FuncionarioViewModel
            {
                FuncionarioDeducoesList = funcionarioDeducoesFiltrados,
                FuncionarioBasicoList = funcionarioBasicoList
            };

            return View("Funcionario", viewModel);
        }


    }

}
