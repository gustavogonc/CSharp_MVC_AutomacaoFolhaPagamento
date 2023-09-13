namespace AutomatizacaoFolhaPagamento.Models
{
    public class CadastroViewModel
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string ConfirmaSenha { get; set; }
        public List<string> Tipo { get; set; }
    }
}
