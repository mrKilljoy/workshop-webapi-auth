namespace Workshop.WebApi.Cookie.Infrastructure.Exceptions;

[Serializable]
public class UserNotFoundException : Exception
{
    public UserNotFoundException(string login) : base($"User '{login}' not found")
    {
    }
}