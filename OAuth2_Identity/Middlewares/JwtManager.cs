using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OAuth2_Identity.Models;

namespace OAuth2_Identity.Middlewares;
public class JwtManager
{
    public string Generate(string secretKey, UserRequestDTO user)
    {
        var tokenHendler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDsecriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),    
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHendler.CreateToken(tokenDsecriptor);
        return "Bearer " + tokenHendler.WriteToken(token);
    }
}