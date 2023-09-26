namespace AutomacaoFolhaPagamento.Models
{
    public class HistPagamentoModel
    {
        public int IdHist { get; set; }
        public int IdFuncionario { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal SalarioBase { get; set; }
        public TimeSpan HoraEx { get; set; }
        public float Beneficios { get; set; }
        public float TotalLiq { get; set; }
    }


}
