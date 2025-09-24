using evecorpfy.Data;
using evecorpfy.Models;
using evecorpfy.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace evecorpfy.ViewsOrganizador
{
    /// <summary>
    /// Interação lógica para CadastroEventos.xaml
    /// </summary>
    public partial class CadastroEventos : UserControl
    {
        public CadastroEventos()
        {
            InitializeComponent();
            CarregarTiposEvento();
            CarregarServicos();
            this.Loaded += CadastroEventos_Loaded;
        }
        private void CadastroEventos_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarEventos();
        }
        private void CarregarTiposEvento()
        {
            var repo = new RepositorioTipoEvento();
            var tiposAtivos = repo.ListarTodos()
                      .Where(te => te.Ativo) // Apenas evento ativos
                      .ToList();

            ComboBoxTipoEvento.ItemsSource = tiposAtivos;
            ComboBoxTipoEvento.DisplayMemberPath = "Nome";
            ComboBoxTipoEvento.SelectedValuePath = "Id";
        }
        private void CarregarEventos()
        {
            var repo = new RepositorioEvento();
            ComboBoxEventos.ItemsSource = repo.ListarPorOrganizador(Sessao.UsuarioId);
        }
        private void TextBoxCEPPrevisualizar(object sender, TextCompositionEventArgs e)
        {
            // Permitir apenas dígitos
            e.Handled = !char.IsDigit(e.Text, 0);
        }
        private void TextBoxCEPIdentador(object sender, TextChangedEventArgs e)
        {
            var textCep = TextBoxCEP.Text.Replace("-", "").Trim();
            if (textCep.Length > 5)
                TextBoxCEP.Text = textCep.Insert(5, "-");
            TextBoxCEP.CaretIndex = TextBoxCEP.Text.Length;
        }
        // Campo para guardar o último endereço retornado pelo ViaCep
        private Endereco? enderecoAtual;
        private async void ButtonBuscarCEP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = new ViaCepService();
                // Remove o hífen para não salvar no banco
                var cep = TextBoxCEP.Text.Replace("-", "").Trim();
                enderecoAtual = await service.BuscarCepAsync(cep);
                if (enderecoAtual != null && !string.IsNullOrWhiteSpace(enderecoAtual.Cep))
                {
                    // Número vem do textbox manual
                    enderecoAtual.Numero = TextBoxNumero.Text.Trim();
                    // Padrão "S/N" se número estiver vazio
                    if (string.IsNullOrWhiteSpace(enderecoAtual.Numero))
                    {
                        enderecoAtual.Numero = "S/N";
                        TextBoxNumero.Text = "S/N"; // garante que o campo seja atualizado na tela também
                    }
                    // Monta o endereço em formato único
                    TextBoxEndereco.Text =
                        $"{enderecoAtual.Logradouro ?? ""}, {enderecoAtual.Numero} - {enderecoAtual.Bairro ?? ""} - {enderecoAtual.Localidade ?? ""}/{enderecoAtual.Uf ?? ""} - {enderecoAtual.Cep}";
                }
                else
                {
                    MessageBox.Show("CEP não encontrado!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar CEP: {ex.Message}", "Erro",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Validação dos campos do formulário
        private Evento? ValidarEvento(bool isUpdate = false)
        {
            if (isUpdate && eventoSelecionado == null)
            {
                MessageBox.Show("Selecione um evento para atualizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (string.IsNullOrWhiteSpace(TextBoxNomeEvento.Text))
            {
                MessageBox.Show("Digite o nome do evento.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (!DatePickerInicio.SelectedDate.HasValue)
            {
                MessageBox.Show("Selecione a data de início.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (!DatePickerFim.SelectedDate.HasValue)
            {
                MessageBox.Show("Selecione a data de fim.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (string.IsNullOrWhiteSpace(TextBoxCapacidade.Text) || !int.TryParse(TextBoxCapacidade.Text.Trim(), out int cap))
            {
                MessageBox.Show("Informe a capacidade máxima (número válido).", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            // Orçamento deve ser decimal, com vírgula
            var cultura = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            string texto = TextBoxOrcamento.Text.Replace("R$", "").Trim();

            if (string.IsNullOrWhiteSpace(texto) ||
                !decimal.TryParse(texto, System.Globalization.NumberStyles.Any, cultura, out decimal orc))
            {
                MessageBox.Show("Informe o orçamento máximo (valor numérico).",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (orc == 0)
            {
                MessageBox.Show("O orçamento máximo não pode ser zero.",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (string.IsNullOrWhiteSpace(TextBoxCEP.Text))
            {
                MessageBox.Show("Informe o CEP.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            // Número pode ser inteiro ou "S/N"
            if (string.IsNullOrWhiteSpace(TextBoxNumero.Text))
            {
                MessageBox.Show("Informe o número do endereço ou 'S/N'.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            string numeroInformado = TextBoxNumero.Text.Trim();
            if (!(numeroInformado.Equals("S/N", StringComparison.OrdinalIgnoreCase) || int.TryParse(numeroInformado, out _)))
            {
                MessageBox.Show("Informe um número válido ou 'S/N'.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (ComboBoxTipoEvento.SelectedValue == null)
            {
                MessageBox.Show("Selecione o tipo de evento.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            var servicosSelecionados = ((List<Servico>)ListBoxServicos.ItemsSource)
                            .Where(s => s.IsSelecionado)
                            .Select(s => s.Id)
                            .ToList();
            // Recupera os serviços selecionados no ListBox           
            return new Evento
            {
                Id = isUpdate ? eventoSelecionado!.Id : 0,
                Nome = TextBoxNomeEvento.Text.Trim(),
                DataInicio = DatePickerInicio.SelectedDate.Value,
                DataFim = DatePickerFim.SelectedDate.Value,
                Capacidade = cap,
                OrcamentoMaximo = orc,
                TipoEventoId = (int)ComboBoxTipoEvento.SelectedValue,
                Cep = TextBoxCEP.Text.Replace("-", "").Trim(),
                Numero = numeroInformado.ToUpper() == "S/N" ? "S/N" : numeroInformado,
                Logradouro = enderecoAtual?.Logradouro ?? "",
                Bairro = enderecoAtual?.Bairro ?? "",
                Localidade = enderecoAtual?.Localidade ?? "",
                Uf = enderecoAtual?.Uf ?? "",
                Estado = $"{enderecoAtual?.Localidade} - {enderecoAtual?.Uf}",
                Observacoes = string.IsNullOrWhiteSpace(TextBoxObservacoes.Text) ? null : TextBoxObservacoes.Text.Trim(),
                Status = isUpdate ? eventoSelecionado!.Status : "EM CADASTRAMENTO",
                OrganizadorId = Sessao.UsuarioId,

                // Agora os serviços selecionados vão junto
                ServicosIds = servicosSelecionados
            };

        }
        private void ButtonCadastrarEvento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var evento = ValidarEvento();
                if (evento == null) return;
                var repo = new RepositorioEvento();
                repo.Inserir(evento);
                MessageBox.Show("Evento cadastrado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarEventos();
                ResetarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar evento: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonAtualizarEvento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var evento = ValidarEvento(isUpdate: true);
                if (evento == null) return;
                var repo = new RepositorioEvento();
                repo.Atualizar(evento);
                MessageBox.Show("Evento atualizado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarEventos();
                ResetarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar evento: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private Evento eventoSelecionado;
        private void ComboBoxEventos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxEventos.SelectedItem is Evento evento)
            {
                eventoSelecionado = evento;
                TextBoxNomeEvento.Text = evento.Nome;
                DatePickerInicio.SelectedDate = evento.DataInicio;
                DatePickerFim.SelectedDate = evento.DataFim;
                TextBoxCEP.Text = evento.Cep;
                TextBoxNumero.Text = evento.Numero;
                TextBoxEndereco.Text = $"{evento.Logradouro}, {evento.Numero} - {evento.Bairro} - {evento.Localidade}/{evento.Uf} - {evento.Cep}";
                TextBoxCapacidade.Text = evento.Capacidade.ToString();
                TextBoxOrcamento.Text = evento.OrcamentoMaximo.ToString("F2");
                ComboBoxTipoEvento.SelectedValue = evento.TipoEventoId;
                TextBoxObservacoes.Text = evento.Observacoes;
                // Recriando o enderecoAtual com dados do banco
                enderecoAtual = new Endereco
                {
                    Cep = evento.Cep,
                    Logradouro = evento.Logradouro,
                    Numero = evento.Numero,
                    Bairro = evento.Bairro,
                    Localidade = evento.Localidade,
                    Uf = evento.Uf
                };
                TextBlockStatus.Text = evento.Status;
                Panel.SetZIndex(ButtonAtualizarEvento, 1);
                var repo = new RepositorioEvento();
                var servicosIds = repo.ListarServicosIdsPorEvento(evento.Id);
                if (ListBoxServicos.ItemsSource is IEnumerable<Servico> servicos)
                {
                    foreach (var servico in servicos)
                    {
                        servico.IsSelecionado = servicosIds.Contains(servico.Id);
                    }
                    // Atualiza a UI
                    ListBoxServicos.Items.Refresh();
                }
            }
        }
        private void ButtonLimpar_Click(object sender, RoutedEventArgs e)
        {
            ResetarCampos();
        }
        private void ResetarCampos()
        {
            TextBoxNomeEvento.Clear();
            DatePickerInicio.SelectedDate = null;
            DatePickerFim.SelectedDate = null;
            TextBoxCEP.Clear();
            TextBoxNumero.Clear();
            TextBoxEndereco.Clear();
            TextBoxCapacidade.Clear();
            TextBoxOrcamento.Clear();
            ComboBoxTipoEvento.SelectedIndex = -1;
            TextBoxObservacoes.Clear();
            eventoSelecionado = null;
            enderecoAtual = null;
            ComboBoxEventos.SelectedIndex = -1;
            Panel.SetZIndex(ButtonAtualizarEvento, -1);

            // Limpa checkboxes de serviços
            if (ListBoxServicos.ItemsSource is IEnumerable<Servico> servicos)
            {
                foreach (var servico in servicos)
                {
                    servico.IsSelecionado = false;
                }
                ListBoxServicos.Items.Refresh(); // Força atualização na UI
            }
        }

        private void DatePickerFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerInicio.SelectedDate.HasValue && DatePickerFim.SelectedDate.HasValue)
            {
                var dataInicio = DatePickerInicio.SelectedDate.Value;
                var dataFim = DatePickerFim.SelectedDate.Value;

                if (dataFim < dataInicio)
                {
                    MessageBox.Show("A data de término não pode ser anterior à data de início!", "Validação de Datas", MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Limpa o valor para forçar correção
                    DatePickerFim.SelectedDate = null;
                }
            }
        }
        //private void TextBoxOrcamento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    // Permitir apenas números e vírgula
        //    e.Handled = !char.IsDigit(e.Text, 0) && e.Text != ",";
        //}
        //private void TextBoxOrcamento_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    var cultura = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");

        //    // Remove "R$" e espaços antes de tentar converter
        //    string texto = TextBoxOrcamento.Text.Replace("R$", "").Trim();

        //    if (decimal.TryParse(texto,
        //                         System.Globalization.NumberStyles.Any,
        //                         cultura,
        //                         out decimal valor))
        //    {
        //        // Reaplica formatação em R$
        //        TextBoxOrcamento.Text = string.Format(cultura, "{0:C}", valor);
        //    }
        //    else
        //    {
        //        if (string.IsNullOrWhiteSpace(texto))
        //        {
        //            TextBoxOrcamento.Text = "R$ 0,00";
        //        }
        //        else
        //        {
        //            MessageBox.Show("Valor inválido. Informe um número válido.",
        //                            "Erro de Formatação",
        //                            MessageBoxButton.OK,
        //                            MessageBoxImage.Warning);
        //            TextBoxOrcamento.Text = "R$ 0,00";
        //        }
        //    }
        //}
        private void TextBoxOrcamento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Permite apenas números
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void TextBoxOrcamento_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = TextBoxOrcamento.Text
                .Replace("R$", "")
                .Replace(".", "")
                .Replace(",", "")
                .Trim();

            if (string.IsNullOrEmpty(txt))
            {
                TextBoxOrcamento.Text = "R$ 0,00";
                TextBoxOrcamento.CaretIndex = TextBoxOrcamento.Text.Length;
                return;
            }

            // Converte para decimal dividindo por 100 (para controlar casas decimais)
            if (decimal.TryParse(txt, out decimal valor))
            {
                valor /= 100;
                TextBoxOrcamento.Text = string.Format(
                    System.Globalization.CultureInfo.GetCultureInfo("pt-BR"),
                    "{0:C}", valor);
                TextBoxOrcamento.CaretIndex = TextBoxOrcamento.Text.Length;
            }
        }

        private void CarregarServicos()
        {
            var repo = new RepositorioTipoServico();
            var servicos = repo.ListarTodos()
                               .Where(s => s.Ativo)
                               .Select(s => new Servico
                               {
                                   Id = s.Id,
                                   Nome = s.Nome,
                                   IsSelecionado = false
                               })
                               .ToList();
            ListBoxServicos.ItemsSource = servicos;
        }
    }
}