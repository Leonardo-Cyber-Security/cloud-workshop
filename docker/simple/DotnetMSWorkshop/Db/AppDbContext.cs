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
                .Property(e => e.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Item>()
                .Property(e => e.Name)
                .HasColumnName("name");

            modelBuilder.Entity<Item>()
                .Property(e => e.Description)
                .HasColumnName("description");
                
            modelBuilder.Entity<Item>()
                .Property(e => e.Quantity)
                .HasColumnName("quantity");
        }
    }
}