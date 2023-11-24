namespace AutomacaoFolhaPagamento.Models
{
    public class ProventosViewModel
    {
        public int? FuncionarioId { get; set; }
        public string? Ano { get; set; }
        public string? Mes { get; set; }
        public DateTime? DataContratacao { get; set; }
        public List<Provento>? Proventos { get; set; }
    }
}
