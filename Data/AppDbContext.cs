using Microsoft.EntityFrameworkCore;
using RuleWay.ProductApi.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RuleWay.ProductApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).IsRequired();
            });
        }

    }
}
