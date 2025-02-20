using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Workshop.WebApi.Authentication.Infrastructure.Filters;

public sealed class CustomGlobalExceptionFilter : IExceptionFilter
{
    private const string MessageText = "Unknown error";
    
    private readonly ILogger<CustomGlobalExceptionFilter> _logger;

    public CustomGlobalExceptionFilter(ILogger<CustomGlobalExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is null)
            _logger.LogError(MessageText);
        else
            _logger.LogError(context.Exception, MessageText);

        context.Result = new ObjectResult(new
        {
            Error = "An unexpected error has occurred."
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        
        context.ExceptionHandled = true;
    }
}