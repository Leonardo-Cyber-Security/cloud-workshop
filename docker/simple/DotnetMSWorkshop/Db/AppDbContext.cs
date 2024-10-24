using Microsoft.EntityFrameworkCore;
using DotnetMSWorkshop.Entities;

namespace DotnetMSWorkshop.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .ToTable("Items"); // Mappa la classe alla tabella Items
        }
    }
}