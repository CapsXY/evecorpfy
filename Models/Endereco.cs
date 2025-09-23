namespace evecorpfy.Models
{
    public class Endereco
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Ibge { get; set; }
        public string Gia { get; set; }
        public string Ddd { get; set; }
        public string Siafi { get; set; }
        // Número do endereço (não vem na API do ViaCEP)
        public string Numero { get; set; }

        // Salvando apenas o que interessa da API do ViaCEP
        public string EnderecoCompleto
        {
            get
            {
                return $"{Logradouro}, {Numero} {Bairro} - {Localidade}/{Uf}";
            }
        }
    }
}