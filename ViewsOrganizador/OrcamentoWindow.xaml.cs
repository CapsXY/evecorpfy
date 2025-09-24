using System.Collections.Generic;
using System.Windows;
using evecorpfy.Models;

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
            // 🔹 Aqui você chamaria o repositório de orçamentos do banco
            // No momento, mock para testar layout
            var listaFake = new List<dynamic>
            {
                new { FornecedorNome = "Buffet Master", Valor = "R$ 5.000,00" },
                new { FornecedorNome = "Buffet Express", Valor = "R$ 4.500,00" }
            };

            DataGridOrcamentos.ItemsSource = listaFake;
        }
    }
}