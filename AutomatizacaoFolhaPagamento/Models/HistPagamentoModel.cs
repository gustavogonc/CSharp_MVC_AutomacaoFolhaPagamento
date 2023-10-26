namespace AutomacaoFolhaPagamento.Models
{
    public class HistPagamentoModel
    {
        public int idHist { get; set; }
        public int idFuncionario { get; set; }
        public DateTime dataPagamento { get; set; }
        public decimal salarioBase { get; set; }
        public TimeSpan horaEx { get; set; }
        public float beneficios { get; set; }
        public float totalLiq { get; set; }
    }


}
