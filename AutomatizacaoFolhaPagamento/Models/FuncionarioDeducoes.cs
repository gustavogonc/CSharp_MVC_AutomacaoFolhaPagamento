namespace AutomacaoFolhaPagamento.Models
{
    public class FuncionarioDeducoes
    {
        public string NomeFuncionario { get; set; }
        public string CPF { get; set; }
        public string Cargo { get; set; }
        public string Departamento { get; set; }
        public decimal INSS { get; set; }
        public decimal FGTS { get; set; }
        public decimal IR { get; set; }
        public decimal VR { get; set; }
        public decimal VT { get; set; }
        public decimal ValorLiquido { get; set; }
        public decimal Salario { get; set; }
        public decimal Descontos { get; set; }
    }
}
