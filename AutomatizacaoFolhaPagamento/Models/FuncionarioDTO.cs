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
            public string nome_funcionario { get; set; }
            public string departamento { get; set; }
        }

        public class Endereco
        {
            public string tipo_endereco { get; set; }

        }

        public class Contato
        {
            public string tipo_telefone { get; set; }

        }
    }
}
