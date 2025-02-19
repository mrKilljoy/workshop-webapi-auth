using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Workshop.WebApi.Authentication.Infrastructure.Swagger;

public class CustomAuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType is null)
            return;
        
        var hasAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                               .OfType<AuthorizeAttribute>().Any() ||
                           context.MethodInfo.GetCustomAttributes(true)
                               .OfType<AuthorizeAttribute>().Any();

        if (!hasAttribute)
            return;

        operation.Security ??= new List<OpenApiSecurityRequirement>();

        var scheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [scheme] = new List<string>()
        });
    }
}