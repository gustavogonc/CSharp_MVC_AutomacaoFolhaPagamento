namespace AutomacaoFolhaPagamento.Models
{
    public class FuncionarioViewModel
    {
        public IEnumerable<FuncionarioDeducoes> funcionarioDeducoesList { get; set; }
        public IEnumerable<Funcionario> funcionarioBasicoList { get; set; }
    }
}
