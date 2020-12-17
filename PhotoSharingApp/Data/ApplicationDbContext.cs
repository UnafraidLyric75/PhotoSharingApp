using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using PhotoSharingApp.Models;

namespace PhotoSharingApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){}

        public DbSet<Photos> Photos { get; set; }

        public DbSet<UserAccount> UserAccount { get; set; }
    }
}
