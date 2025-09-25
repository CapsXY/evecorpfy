using evecorpfy.Data;
using evecorpfy.Models;
using System.Windows;
namespace evecorpfy.ViewsOrganizador
{
    public partial class OrcamentoWindow : Window
    {
        private TipoServico _servico;
        private Evento _evento;
        public OrcamentoWindow(TipoServico servico, Evento evento)
        {
            InitializeComponent();
            _servico = servico;
            _evento = evento;

            TxtServico.Text = $"Serviço: {servico.Nome} (Evento: {evento.Nome})";
            CarregarOrcamentos();
        }
        private void CarregarOrcamentos()
        {
            var repo = new RepositorioEventoProposta();
            var propostas = repo.ListarPropostasPorServico(_evento.Id, _servico.Id);

            DataGridOrcamentos.ItemsSource = propostas;
        }
    }
}