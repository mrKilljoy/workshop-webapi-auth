namespace Workshop.WebApi.Authentication.Infrastructure.Exceptions;

[Serializable]
public class UserNotFoundException : Exception
{
    public UserNotFoundException(string login) : base($"User '{login}' not found")
    {
    }
}