using OAuth2_Identity.Entities;

namespace OAuth2_Identity.Entities;

public class Role : EntityBase
{
    public int Id { get; set; }
    public string Description { get; set; }
    public User User { get; set; }
}