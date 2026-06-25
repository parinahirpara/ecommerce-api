using EcommerceAPI.Models;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EcommerceAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Staff>().HasIndex(s => s.Email).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.CategoryName).IsUnique();
            modelBuilder.Entity<SubCategory>().HasIndex(s => s.SubCategoryName).IsUnique();
            modelBuilder.Entity<SubCategory>().HasOne(s => s.Category).WithMany(c => c.SubCategories).HasForeignKey(s => s.CategoryId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Material>().HasIndex(m => m.MaterialName).IsUnique();
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>().HasOne(p => p.SubCategory).WithMany().HasForeignKey(p => p.SubCategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProductVariant>().HasOne(v => v.Product).WithMany(p => p.Variants).HasForeignKey(v => v.ProductId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductVariant>().HasOne(v => v.Material).WithMany().HasForeignKey(v => v.MaterialId).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<ProductVariant>(entity =>
            //{
            //    entity.Property(e => e.Size)
            //        .HasConversion(
            //            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            //            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            //        );
            //});
            modelBuilder.Entity<ProductImage>().HasOne(img => img.Product).WithMany(p => p.Images).HasForeignKey(img => img.ProductId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductImage>().HasOne(img => img.ProductVariant).WithMany(v => v.Images).HasForeignKey(img => img.ProductVariantId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<ProductVariant>().Property(v => v.Price).HasPrecision(18, 2);
            modelBuilder.Entity<ProductVariant>().Property(v => v.Weight).HasPrecision(18, 2);
            modelBuilder.Entity<ProductVariant>().Property(v => v.DiscountPercentage).HasPrecision(5, 2);
        }
    }
}
