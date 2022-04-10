namespace OAuth2_Identity.Models;

public class Settings
{

    public Jwt Jwt { get; set; }
        
}

public class Jwt
{
    public string Issuer { get; set; }
    public string Key { get; set; }
    public string ExpiryMinutes { get; set; }
    public bool ValidateLifetime { get; set; }
        
}