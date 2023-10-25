namespace AutomacaoFolhaPagamento.Models
{
    public class CargoDetalhes
    {
        public int id_cargo { get; set; }
        public string nome_cargo { get; set; }
        public string descricao_cargo { get; set; }
        public decimal salario { get; set; }
        public string nome_departamento { get; set; }
        public int id_departamento { get; set; }
        public int? DepartamentoId { get; set; }
    }
}
