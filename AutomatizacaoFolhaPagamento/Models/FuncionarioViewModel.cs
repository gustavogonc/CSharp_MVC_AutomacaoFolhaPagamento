namespace AutomacaoFolhaPagamento.Models
{
    public class FuncionarioViewModel
    {
        public IEnumerable<FuncionarioDeducoes> FuncionarioDeducoesList { get; set; }
        public IEnumerable<Funcionario> FuncionarioBasicoList { get; set; }
    }
}
