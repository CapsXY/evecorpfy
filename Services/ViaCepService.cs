using evecorpfy.Models;
using System.Net.Http;
using System.Text.Json;
namespace evecorpfy.Services
{
    public class ViaCepService
    {
        private static readonly HttpClient client = new HttpClient();
        public async Task<Endereco> BuscarCepAsync(string cep)
        {
            string url = $"https://viacep.com.br/ws/{cep}/json/";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var endereco = JsonSerializer.Deserialize<Endereco>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return endereco;
        }
    }
}


