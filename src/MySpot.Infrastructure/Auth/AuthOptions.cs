namespace MySpot.Infrastructure.Auth;

public class AuthOptions
{
    public string Issuer { get; set; } // iss field (who created this token, some server/service name)
    public string Audience { get; set; } //aud field (for whom this token was created, for some service/user/another server)
    public string SigningKey { get; set; }
    public TimeSpan? Expiry { get; set; }
}