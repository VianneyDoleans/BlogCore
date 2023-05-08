namespace DBAccess.Settings
{
    public class TokenSettings
    {
        public string Issuer { get; set; }

        public string Secret { get; set; }

        public int AccessTokenExpirationInMinutes { get; set; }
        
        public int RefreshTokenExpirationInMinutes { get; set; }
    }
}
