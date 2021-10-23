using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using clothing_store_api.Models;
using clothing_store_api.Models.Customers;
using clothing_store_api.Models.Base;
using clothing_store_api.Models.Products;

namespace clothing_store_api.Data
{
    public class SystemContext : DbContext
    {
        public SystemContext(DbContextOptions<SystemContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Manager> Managers { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<ShippingInfo> ShippingInfos { get; set; }
        public DbSet<CartPosition> CartPositions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Models.Base.Type> Types { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Raw> Raws { get; set; }


    }
}
