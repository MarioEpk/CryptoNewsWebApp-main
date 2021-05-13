using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication.Models;

namespace CryptoNewsWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CryptoNewsWebApp.Models.DataSource> DataSource { get; set; }
        public DbSet<CryptoNewsWebApp.Models.Post> Post { get; set; }
        public DbSet<WebApplication.Models.CMCCoin> Coin { get; set; }

    }
}
