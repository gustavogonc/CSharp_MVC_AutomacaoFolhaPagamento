namespace AutomacaoFolhaPagamento.Models
{
    public class FuncionarioDTO
    {
        public FuncionarioDetalhe funcionario { get; set; }
        public List<Endereco> enderecos { get; set; }
        public List<Contato> contatos { get; set; }

        public class FuncionarioDetalhe
        {
            public int id_funcionario { get; set; }
            public string? nome_funcionario { get; set; }
            public string? sexo { get; set; }
            public string? estado_civil { get; set; }
            public int? cargo_id { get; set; }
            public string? departamento { get; set; }
            public string? cargo { get; set; }
            public DateTime data_contratacao { get; set; }
            public string? cpf { get; set; }
        }

        public class Endereco
        {
            public string? tipo_endereco { get; set; }
            public string? rua { get; set; }
            public string? bairro { get; set; }
            public string? cep { get; set; }
            public string? cidade { get; set; }
        }

        public class Contato
        {
            public string? tipo_telefone { get; set; }
            public string? numero_contato { get; set; }
        }
    }
}
