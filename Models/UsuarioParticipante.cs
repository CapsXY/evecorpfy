namespace evecorpfy.Models
{
    public class UsuarioParticipante
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int TipoParticipanteId { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
    }
}
