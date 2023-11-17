namespace AutomacaoFolhaPagamento.Models
{
    public class HistPagamentoModel
    {
        public int idHist { get; set; }
        public int idFuncionario { get; set; }
        public DateTime dataPagamento { get; set; }
        public decimal valor { get; set; }

        public string texto { get; set; }
    }


}
