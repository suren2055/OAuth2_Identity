using OAuth2_Identity.Entities;

namespace OAuth2_Identity.Core.Repositories;

public class Role : EntityBase
{
    public int Id { get; set; }
    public string Description { get; set; }
    public User User { get; set; }
}