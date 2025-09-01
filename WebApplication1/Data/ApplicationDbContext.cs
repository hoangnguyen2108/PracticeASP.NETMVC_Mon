using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CategoryId);

            builder.Entity<Category>()
                .HasData(new Category
                {
                    CategoryId = 1,
                    CategoryName = "Name1",

                }, new Category
                {
                    CategoryId = 2,
                    CategoryName = "Name2"
                });

            builder.Entity<Product>()
                .HasData(new Product
                {
                    ProductId = 1,
                    ProductName = "PName1",
                    Quantity = 1,
                    CategoryId =2
                }, new Product
                {
                    ProductId = 2,
                    ProductName = "PName2",
                    Quantity = 5,
                    CategoryId = 1
                }, new Product
                {
                    ProductId = 3,
                    ProductName = "PName3",
                    Quantity = 10,
                    CategoryId = 1
                });
                
        }
    }
}
