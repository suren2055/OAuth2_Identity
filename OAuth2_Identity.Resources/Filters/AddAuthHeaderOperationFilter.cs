using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OAuth2_Identity.Recourses.Filters;

public class AddAuthHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var isAuthorized = (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<Auth>().Any()
                            && !context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                .OfType<AllowAnonymousAttribute>().Any())
                           || (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>()
                                   .Any()
                               && !context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                   .OfType<AllowAnonymousAttribute>().Any());
        if (!isAuthorized) return;
        operation.Responses.TryAdd("401", new OpenApiResponse {Description = "Unauthorized"});
        operation.Responses.TryAdd("403", new OpenApiResponse {Description = "Forbidden"});
        var jwtBearerScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "bearer"
            }
        };
        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement {[jwtBearerScheme] = new string[] { }}
        };
    }
}