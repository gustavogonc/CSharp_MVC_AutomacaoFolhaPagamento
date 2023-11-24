namespace AutomacaoFolhaPagamento.Models.ApiDTOs
{
    public class EnderecoApiDTO
    {
        public int id { get; set; }
        public string? rua { get; set; }
        public string? tipo_endereco { get; set; }
        public string? num_endereco { get; set; }
        public string? bairro { get; set; }
        public string? cep { get; set; }
        public string? cidade { get; set; }
        public string? uf_estado { get; set; }
    }
}
