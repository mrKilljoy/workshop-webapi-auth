using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Workshop.Shared.Configuration;
using Workshop.Shared.Data;
using Workshop.Shared.Models;

namespace Workshop.Shared.Services;

public class RefreshTokenManager : ITokenManager
{
    private readonly ILogger<RefreshTokenManager> _logger;
    private readonly IOptions<RefreshTokenManagerOptions> _options;
    private readonly UserDbContext _dbContext;

    public RefreshTokenManager(
        ILogger<RefreshTokenManager> logger,
        IOptions<RefreshTokenManagerOptions> options,
        UserDbContext dbContext)
    {
        _logger = logger;
        _options = options;
        _dbContext = dbContext;
    }
    
    public async Task<bool> IsValid(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
                return false;
        
            var item = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Value.Equals(token));
            return item is not null && item.IsValid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(IsValid));
            throw;
        }
    }

    public async Task<RefreshToken> GetToken(Guid userId)
    {
        try
        {
            var item = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);

            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(GetToken));
            throw;
        }
    }

    public async Task<string> CreateToken(Guid userId)
    {
        try
        {
            var tokenValue = Guid.NewGuid().ToString("n");
            var token = new RefreshToken()
            {
                UserId = userId,
                Value = tokenValue,
                ExpiresAt = DateTime.UtcNow.AddSeconds(GetTokenLifetimeSeconds())
            };
            var result = await _dbContext.RefreshTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();

            return result?.Entity?.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateToken));
            throw;
        }
    }

    public async Task<bool> RemoveToken(Guid userId)
    {
        try
        {
            var item = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
            if (item is null)
                return false;

            _dbContext.RefreshTokens.Remove(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(RemoveToken));
            throw;
        }
    }

    private int GetTokenLifetimeSeconds()
    {
        var lifetimeSeconds = _options?.Value?.RefreshTokenLifetimeSeconds;
        return lifetimeSeconds ?? 0;
    }
}