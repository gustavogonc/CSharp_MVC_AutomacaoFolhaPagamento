namespace AutomacaoFolhaPagamento.Models
{
    public class GraficosViewModel
    {
        public IEnumerable<DashboardModel> pagamentoMensal { get; set; }
        public IEnumerable<DashboardModel> pagamentoAnoMes { get; set; }
    }
}
