using Microsoft.EntityFrameworkCore;
using OAuth2_Identity.Resources.Entities;

namespace OAuth2_Identity.Resources.Core.Concrete;

public class EFDBContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost;
                                                        Database=db_OAuth2_Resources;
                                                        User Id=;
                                                        Password=;
                                                        Trusted_Connection=False;
                                                        MultipleActiveResultSets=true");
    }
}