namespace evecorpfy.Models
{
    public class ParticipanteAgendaResumo
    {
        public string ParticipanteNome { get; set; }
        public int QtdeEventos { get; set; }
    }

    public class FornecedorResumo
    {
        public string FornecedorNome { get; set; }
        public decimal TotalGasto { get; set; }
    }

    public class TipoParticipanteResumo
    {
        public string TipoNome { get; set; }
        public int Qtde { get; set; }
    }

    public class SaldoEventoResumo
    {
        public string EventoNome { get; set; }
        public decimal OrcamentoMax { get; set; }
        public decimal TotalPropostas { get; set; }
    }
}
