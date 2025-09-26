namespace evecorpfy.Models
{
    public class EventoProposta
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public int ServicoId { get; set; }
        public int FornecedorUsuarioId { get; set; }
        public decimal ValorProposta { get; set; }
        public DateTime DataProposta { get; set; }
        public string NomeServico { get; set; }
        public bool TemOrcamento { get; set; }

        public string TextoBotao => TemOrcamento ? "Cancelar Orçamento" : "Negociar Serviços";
    }
}