using evecorpfy.Data;
using evecorpfy.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace evecorpfy.ViewsOrganizador
{
    public partial class DashboardOrganizador : UserControl
    {
        public SeriesCollection AgendaSeries { get; set; }
        public string[] AgendaLabels { get; set; }

        public SeriesCollection FornecedoresSeries { get; set; }

        public SeriesCollection TiposParticipantesSeries { get; set; }

        public SeriesCollection OrcamentoSeries { get; set; }
        public string[] OrcamentoLabels { get; set; }
        public Func<double, string> ValorFormatter { get; set; }

        private readonly RepositorioDashboardOrganizador _repo;

        public DashboardOrganizador()
        {
            InitializeComponent();
            _repo = new RepositorioDashboardOrganizador();
            CarregarDados();
            DataContext = this;
        }

        private void CarregarDados(DateTime? inicio = null, DateTime? fim = null)
        {
            // 1) Agenda de Participantes
            var agenda = _repo.ContarEventosPorParticipante(inicio, fim);
            AgendaLabels = agenda.Select(a => a.ParticipanteNome).ToArray();
            AgendaSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Eventos",
                    Values = new ChartValues<int>(agenda.Select(a => a.QtdeEventos))
                }
            };

            // 2) Fornecedores mais utilizados
            FornecedoresSeries = new SeriesCollection();
            foreach (FornecedorResumo f in _repo.ContarPorFornecedor(inicio, fim))
            {
                FornecedoresSeries.Add(new PieSeries
                {
                    Title = f.FornecedorNome,
                    Values = new ChartValues<decimal> { f.TotalGasto },
                    DataLabels = true
                });
            }

            // 3) Tipos de participantes
            TiposParticipantesSeries = new SeriesCollection();
            foreach (TipoParticipanteResumo t in _repo.ContarPorTipoParticipante(inicio, fim))
            {
                TiposParticipantesSeries.Add(new PieSeries
                {
                    Title = t.TipoNome,
                    Values = new ChartValues<int> { t.Qtde },
                    DataLabels = true
                });
            }

            // 4) Saldo de orçamento
            var saldo = _repo.ObterSaldoEventos(Sessao.UsuarioId, inicio, fim);
            OrcamentoLabels = saldo.Select(s => s.EventoNome).ToArray();
            OrcamentoSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Orçamento Máx.",
                    Values = new ChartValues<decimal>(saldo.Select(s => s.OrcamentoMax))
                },
                new ColumnSeries
                {
                    Title = "Total Propostas",
                    Values = new ChartValues<decimal>(saldo.Select(s => s.TotalPropostas))
                }
            };

            ValorFormatter = v => v.ToString("C");
        }
        private void AplicarFiltro_Click(object sender, RoutedEventArgs e)
        {
            DateTime? inicio = DateInicio.SelectedDate;
            DateTime? fim = DateFim.SelectedDate;

            CarregarDados(inicio, fim);

            // força refresh do binding
            DataContext = null;
            DataContext = this;
        }

        private void LimparFiltro_Click(object sender, RoutedEventArgs e)
        {
            DateInicio.SelectedDate = null;
            DateFim.SelectedDate = null;

            CarregarDados();

            DataContext = null;
            DataContext = this;
        }
    }
}
