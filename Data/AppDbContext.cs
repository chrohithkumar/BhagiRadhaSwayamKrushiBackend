using Microsoft.EntityFrameworkCore;
using BhagiRadhaSwayamKrushi.Models;
using System.Collections.Generic;

namespace BhagiRadhaSwayamKrushi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

    }
}
