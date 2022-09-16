using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Models
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory
                {
                    Id=1,
                    Name="Hoodies"
                },
                new ProductCategory
                {
                    Id = 2,
                    Name = "Jampers"
                },
                new ProductCategory
                {
                    Id = 3,
                    Name = "Skirts"
                }

                );
            */
            
            
            modelBuilder.Entity<ProductA>((pc =>
            {
                pc.ToView("ProductsA");
            }));
            modelBuilder.Entity<OrderV>((pc =>
            {
                pc.ToView("OrdersV");
            }));
            
            
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<ProductA> ProductsA { get; set; }
        public DbSet<OrderV> OrdersV { get; set; }



    }
}
