namespace AutomacaoFolhaPagamento.Models
{
    public class Usuarios
    {
        public int usuarioId { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
        public Int16? ativo { get; set; }
        public Int16? administrador { get; set; }
        public string? token { get; set; }
        public DateTime? expiration_token { get; set; }
    }

}
