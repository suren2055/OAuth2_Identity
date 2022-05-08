namespace OAuth2_Identity.Models;

public class ClientRequestDTO
{
    public Guid ClientId { get; set; }
    public string ClientKey { get; set; }
    public Guid UserSecret { get; set; }
    public string ClientName { get; set; }
}