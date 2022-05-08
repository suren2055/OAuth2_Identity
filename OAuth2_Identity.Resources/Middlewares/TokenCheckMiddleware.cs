using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OAuth2_Identity.Recourses.Middlewares;

public class TokenCheckMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public TokenCheckMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization",out _))
            AttachUserToContext(context);
        await _next(context);
    }

    private void AttachUserToContext(HttpContext context)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Settings:Jwt:Key"]);
            tokenHandler.ValidateToken(context.Request.Headers["Authorization"], new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            // attach user to context on successful jwt validation
            context.Items["sub"] = jwtToken.Claims.First(x => x.Type == "sub").Value;
            context.Items["email"] = jwtToken.Claims.First(x => x.Type == "email").Value;
        }
        catch (Exception ex)
        {
            var e = ex.Message;
        }
    }
}