namespace Workshop.Shared.Configuration;

public class RefreshTokenManagerOptions
{
    public const string SectionName = "Security";

    public int RefreshTokenLifetimeSeconds { get; set; }
}