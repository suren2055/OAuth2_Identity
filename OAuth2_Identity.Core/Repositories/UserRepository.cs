using OAuth2_Identity.Core.Concrete;
using OAuth2_Identity.Entities;

namespace OAuth2_Identity.Core.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly EFDBContext _context;

    public UserRepository(EFDBContext context) : base(context)
    {
        _context = context;
    }
}