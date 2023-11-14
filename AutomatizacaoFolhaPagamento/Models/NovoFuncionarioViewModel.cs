namespace AutomacaoFolhaPagamento.Models
{
    public class NovoFuncionarioViewModel
    {
        public string nome { get; set; }
        public string? cpf { get; set; }
        public string? sexo { get; set; }
        public int? cargo_id { get; set; }
        public DateTime? data_contratacao { get; set; }
        public string? estado_civil { get; set; }
        public string? rua { get; set; }
        public string? tipo_endereco { get; set; }
        public string? num_endereco { get; set; }
        public string? bairro { get; set; }
        public string? cep { get; set; }
        public string? cidade { get; set; }
        public string? uf_estado { get; set; }
        public string? tipo_telefone { get; set; }
        public string? numero_contato { get; set; }
        public string? email_usuario { get; set; }
    }
}
