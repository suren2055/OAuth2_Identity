using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OAuth2_Identity.Authentication;

namespace OAuth2_Identity.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    //private readonly Settings _settings;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token!=null)
            AttachUserToContext(context, userService,token);

        await _next(context);


    }

    public void AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var tokenHendler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Settings:Jwt:Key"]);
            tokenHendler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero

            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var uname = jwtToken.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;
            context.Items["User"] = userService.GetUser(uname);
        }
        catch (Exception ex)
        {
            var e = ex.Message;
        }
    }
}