using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Crypto.Models;

namespace Crypto.WebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Crypto.WebApplication.Models.DataSource> DataSource { get; set; }
        public DbSet<Crypto.WebApplication.Models.Post> Post { get; set; }
        public DbSet<Crypto.Models.CMCCoin> Coin { get; set; }

    }
}
