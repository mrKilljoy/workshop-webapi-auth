using Workshop.Shared.Models;

namespace Workshop.Shared.Services;

public interface ITokenManager
{
    Task<bool> IsValid(string token);
    
    Task<RefreshToken> GetToken(Guid userId);

    Task<string> CreateToken(Guid userId);

    Task<bool> RemoveToken(Guid userId);
}