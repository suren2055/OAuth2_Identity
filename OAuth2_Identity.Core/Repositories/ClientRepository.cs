using OAuth2_Identity.Core.Concrete;
using OAuth2_Identity.Entities;

namespace OAuth2_Identity.Core.Repositories;

public class ClientRepository : RepositoryBase<Client>, IClientRepository
{
    private readonly EFDBContext _context;

    public ClientRepository(EFDBContext context) : base(context)
    {
        _context = context;
    }
}