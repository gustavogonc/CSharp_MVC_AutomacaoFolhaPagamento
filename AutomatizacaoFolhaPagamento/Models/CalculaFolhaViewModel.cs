namespace AutomacaoFolhaPagamento.Models
{
    public class CalculaFolhaViewModel
    {
        public List<FuncionariosCalculo> Funcionarios { get; set; }
        public List<FuncionarioDetalhes> DetalhesFuncionario { get; set; }
        public int SelecionadoFuncionarioId { get; set; }
    }
}