namespace evecorpfy
{
    public static class ConexaoFactory
    {
        public static string GetConnectionString()
        {
            var c = ConfigManager.Carregar();
            if (c == null) throw new Exception("Configuração de conexão não encontrada.");
            return $"Server={c.Servidor};Database={c.Banco};User Id={c.Usuario};Password={c.Senha};TrustServerCertificate=True;";
        }
    }
}
