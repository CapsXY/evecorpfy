namespace evecorpfy.Security
{
    public static class PasswordHasher
    {
        private const int WorkFactor = 12;
        public static string Hash(string senhaPura)
            => BCrypt.Net.BCrypt.HashPassword(senhaPura, workFactor: WorkFactor);
        public static bool Verify(string senhaPura, string hash)
            => BCrypt.Net.BCrypt.Verify(senhaPura, hash);
    }
}