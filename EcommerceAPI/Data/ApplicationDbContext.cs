using EcommerceAPI.Models;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Staff>().HasIndex(s => s.Email).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.CategoryName).IsUnique();
            modelBuilder.Entity<SubCategory>().HasIndex(s => s.SubCategoryName).IsUnique();
            modelBuilder.Entity<SubCategory>().HasOne(s => s.Category).WithMany(c => c.SubCategories).HasForeignKey(s => s.CategoryId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductVariant>().HasOne(v => v.Product).WithMany(p => p.Variants).HasForeignKey(v => v.ProductId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductImage>().HasOne(img => img.Product).WithMany(p => p.Images).HasForeignKey(img => img.ProductId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductImage>().HasOne(img => img.ProductVariant).WithMany(v => v.Images).HasForeignKey(img => img.ProductVariantId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
