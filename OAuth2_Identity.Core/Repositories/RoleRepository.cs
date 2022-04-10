using OAuth2_Identity.Core.Concrete;

namespace OAuth2_Identity.Core.Repositories;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    private readonly CoreDBContext _context;

    public RoleRepository(CoreDBContext context) : base(context)
    {
        _context = context;
    }
}