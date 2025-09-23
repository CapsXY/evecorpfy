namespace evecorpfy.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Estado { get; set; }
        public string Observacoes { get; set; }
        public int Capacidade { get; set; }
        public decimal OrcamentoMaximo { get; set; }
        public int OrganizadorId { get; set; }
        public int TipoEventoId { get; set; }
        public string Status { get; set; }
    }
}