using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MobieStoreWeb.Models;

namespace MobieStoreWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            builder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);

            builder.Entity<Product>()
                .Property(p => p.Status)
                .HasConversion(
                value => value.ToString(),
                value => (ProductStatus)Enum.Parse(typeof(ProductStatus), value));


            builder.Entity<Order>()
                .Property(p => p.Status)
                .HasConversion(
                value => value.ToString(),
                value => (OrderStatus)Enum.Parse(typeof(OrderStatus), value));  
            builder.Entity<Order>()
                .Property(p => p.PaymentStatus)
                .HasConversion(
                value => value.ToString(),
                value => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), value));
              builder.Entity<Order>()
                .Property(p => p.DeliveryOption)
                .HasConversion(
                value => value.ToString(),
                value => (DeliveryOption)Enum.Parse(typeof(DeliveryOption), value));
              builder.Entity<Order>()
                .Property(p => p.PaymentMethod)
                .HasConversion(
                value => value.ToString(),
                value => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), value));
            
            InitData(builder);
            base.OnModelCreating(builder);
        }

        private void InitData(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
           new Category
           {
               Id = 1,
               Name = "Mobile"
           },
           new Category
           {
               Id = 2,
               Name = "Tablet"
           },
           new Category
           {
               Id = 3,
               Name = "Watch"
           },
           new Category
           {
               Id = 4,
               Name = "Laptop"
           },
           new Category
           {
               Id = 5,
               Name = "Accessory"
           });
            builder.Entity<Manufacturer>().HasData(
            new Manufacturer
            {
                Id = 1,
                Name = "Apple"
            },
            new Manufacturer
            {
                Id = 2,
                Name = "Samsung"
            },
            new Manufacturer
            {
                Id = 3,
                Name = "Oppo"
            },
            new Manufacturer
            {
                Id = 4,
                Name = "Xiaomi"
            },
            new Manufacturer
            {
                Id = 5,
                Name = "Huawei"
            });
        }
    }
}
