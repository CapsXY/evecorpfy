using Newtonsoft.Json;
using System.IO;
namespace evecorpfy
{
    public static class ConfigManager
    {
        private static string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "evecorpfy_config.json");

        public static void Salvar(ConfiguracaoConexao config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static ConfiguracaoConexao Carregar()
        {
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ConfiguracaoConexao>(json);
        }
    }
}