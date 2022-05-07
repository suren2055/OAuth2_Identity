using OAuth2_Identity.Core.Concrete;

namespace OAuth2_Identity.Core.Repositories;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    private readonly EFDBContext _context;

    public RoleRepository(EFDBContext context) : base(context)
    {
        _context = context;
    }
}