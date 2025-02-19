namespace Workshop.WebApi.Cookie.Infrastructure.Configuration;

public class SecurityOptions
{
    public const string SectionName = "Security";
    
    public string EncryptionKey { get; set; }

    public int RefreshTokenLifetimeSeconds { get; set; }
}