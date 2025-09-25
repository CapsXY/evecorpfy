using System.Windows;
using evecorpfy.Models;

namespace evecorpfy.ViewsOrganizador
{
    public partial class ParticipantesWindow : Window
    {
        public ParticipantesWindow(List<ParticipanteEvento> participantes)
        {
            InitializeComponent();
            DataGridParticipantes.ItemsSource = participantes;
        }
    }
}
