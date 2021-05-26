using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DataSource> DataSource { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<CMCCoin> Coin { get; set; }

    }
}
