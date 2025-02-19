namespace Workshop.Shared.Models;

public class RefreshToken
{
    public Guid UserId { get; set; }
    
    public string Value { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsValid() => ExpiresAt > DateTime.UtcNow;
}