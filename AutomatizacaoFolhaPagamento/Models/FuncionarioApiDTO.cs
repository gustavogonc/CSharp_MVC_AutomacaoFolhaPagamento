using System;

namespace AutomacaoFolhaPagamento.Models
{
    public class FuncionarioApiDTO
    {
        public FuncionarioApiDTO()
        {
            enderecos = new List<EnderecoApiDTO>();
            telefones = new List<TelefoneApiDTO>();
        }

        public string? nome { get; set; }
        public string? cpf { get; set; }
        public string? sexo { get; set; }
        public int? cargo_id { get; set; }
        public DateTime? data_contratacao { get; set; }
        public string? estado_civil { get; set; }
        public List<EnderecoApiDTO>? enderecos { get; set; }
        public List<TelefoneApiDTO>? telefones { get; set; }
    }
}
