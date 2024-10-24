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
                .ToTable("items"); // Mappa la classe alla tabella Items
            modelBuilder.Entity<Item>()
                .Property(e => e.Name)
                .HasColumnName("name");
        }
    }
}