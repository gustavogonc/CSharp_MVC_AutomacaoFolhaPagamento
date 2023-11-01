namespace AutomacaoFolhaPagamento.Models
{
    public class FuncionarioDetalhes
    {
        public string nomeFuncionario { get; set; }
        public string departamento { get; set; }
        public string cargo { get; set; }
        public decimal salario { get; set; }
        public DateTime dataContratacao { get; set; }
        public decimal descontoINSS { get; set; }
        public decimal descontoIRRF { get; set; }

    }
}
