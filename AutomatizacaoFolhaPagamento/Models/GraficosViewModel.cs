namespace AutomacaoFolhaPagamento.Models
{
    public class GraficosViewModel
    {
        public IEnumerable<DashboardModel> PagamentoMensal { get; set; }
        public IEnumerable<DashboardModel> PagamentoAnoMes { get; set; }
    }
}
