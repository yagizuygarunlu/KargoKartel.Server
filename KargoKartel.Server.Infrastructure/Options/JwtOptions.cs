namespace KargoKartel.Server.Infrastructure.Options
{
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public int ExpiryMinutes { get; set; } = 60;
    }
}
