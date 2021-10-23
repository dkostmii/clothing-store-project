using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using clothing_store_api.Models;
using clothing_store_api.Models.Customers;
using clothing_store_api.Models.Products;
using clothing_store_api.Models.Base;

namespace clothing_store_api.Data
{
    public class InitContext : DbContext
    {
        public InitContext(DbContextOptions<InitContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }


        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Models.Base.Type> Types { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Raw> Raws { get; set; }



        public DbSet<Order> Orders { get; set; }
        public DbSet<ShippingInfo> ShippingInfos { get; set; }
        public DbSet<CartPosition> CartPositions { get; set; }



        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Manager> Managers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>().ToTable("Colors");
            modelBuilder.Entity<Models.Base.Type>().ToTable("Types");
            modelBuilder.Entity<Raw>().ToTable("Raws");
            modelBuilder.Entity<Size>().ToTable("Sizes");
            modelBuilder.Entity<Material>().ToTable("Materials");
            modelBuilder.Entity<ProductImage>().ToTable("ProductImages");

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Manager>().ToTable("Managers");

            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<ShippingInfo>().ToTable("ShippingInfos");
            modelBuilder.Entity<CartPosition>().ToTable("CartPositions");
        }
    }
}
