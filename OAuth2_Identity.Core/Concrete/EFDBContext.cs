using Microsoft.EntityFrameworkCore;
using OAuth2_Identity.Entities;

namespace OAuth2_Identity.Core.Concrete;

public class EFDBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost;
                                                        Database=db_OAuth2_Identity;
                                                        User Id=;
                                                        Password=;
                                                        Trusted_Connection=False;
                                                        MultipleActiveResultSets=true");
    }
}