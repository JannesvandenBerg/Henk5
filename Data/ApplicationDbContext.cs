using Henk5.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Henk5.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Category> categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}