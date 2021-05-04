using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CryptoNewsWebApp.Models;

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
        
    }
}
