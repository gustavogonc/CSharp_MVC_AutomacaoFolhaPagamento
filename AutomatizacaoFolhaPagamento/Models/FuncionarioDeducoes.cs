namespace AutomacaoFolhaPagamento.Models
{
    public class FuncionarioDeducoes
    {
        public string nomeFuncionario { get; set; }
        public string cpf { get; set; }
        public string cargo { get; set; }
        public string departamento { get; set; }
        public decimal inss { get; set; }
        public decimal fgts { get; set; }
        public decimal ir { get; set; }
        public decimal vr { get; set; }
        public decimal vt { get; set; }
        public decimal valorLiquido { get; set; }
        public decimal salario { get; set; }
        public decimal descontos { get; set; }
    }
}
