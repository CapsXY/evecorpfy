namespace evecorpfy.Models
{
    public class UsuarioFornecedor
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int TipoParticipanteId { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
    }
}
