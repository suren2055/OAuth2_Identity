using Microsoft.EntityFrameworkCore;
using OAuth2_Identity.Entities;

namespace OAuth2_Identity.Core.Concrete;

public class CoreDBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost;
                                                        Database=db_OAuth2;
                                                        User Id=sql;
                                                        Password=OAuth2Sql123;
                                                        Trusted_Connection=False;
                                                        MultipleActiveResultSets=true");
    }
}