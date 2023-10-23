namespace AutomacaoFolhaPagamento.Models
{
    public class CargoDTO
    {
        public int id_cargo { get; set; }
        public string nome_cargo { get; set; }
        public string descricao_cargo { get; set; }
        public decimal salario { get; set; }
        public int departamentoId { get; set; }
        public string departamento { get; set; }
    }
}
