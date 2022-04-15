using EShop.Domain.DomainModels;
using EShop.Domain.DomainModels.Relations;
using EShop.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace EShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<EShopApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductInOrder> ProductInOrders { get; set; }
        

        // Fluent Configuration API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            /**
            * Foreign key for the one-to-one relationship 
            * between User and ShoppingCart
            */
            builder.Entity<ShoppingCart>()
                .HasOne<EShopApplicationUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);
            /**
             * Composite primary key for the ProductInShoppingCart
             * many-to-many relationship class between Product and ShoppingCart
             */
            // Napravivme ProductInShoppingCart da nasleduva od BaseEntity veke si ima svoj ID ne mu treba composite key
            //builder.Entity<ProductInShoppingCart>()
            //    .HasKey(z => new { z.ProductId, z.ShoppingCartId });

            /**
             * Foreign key to the ShoppingCart for a Product 
             */
            builder.Entity<ProductInShoppingCart>()
                .HasOne(z => z.Product)
                .WithMany(z => z.ProductInShoppingCart)
                .HasForeignKey(z => z.ShoppingCartId);

            /**
             * Foreign key to the Product for a ShoppingCart 
             */
            builder.Entity<ProductInShoppingCart>()
                .HasOne(z => z.ShoppingCart)
                .WithMany(z => z.ProductInShoppingCart)
                .HasForeignKey(z => z.ProductId);


            builder.Entity<Order>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            /**
             * Composite primary key for the ProductInOrder
             * many-to-many relationship class between Product and Order
             */
            // Napravivme ProductInOrder da nasleduva od BaseEntity veke si ima svoj ID ne mu treba composite key
            //builder.Entity<ProductInOrder>()
            //    .HasKey(z => new { z.ProductId, z.OrderId });

            /**
            * Foreign key to the Product for a Order 
            */
            builder.Entity<ProductInOrder>()
                .HasOne(z => z.SelectedProduct)
                .WithMany(z => z.Orders)
                .HasForeignKey(z => z.ProductId);

            /**
             * Foreign key to the Order for a Product 
             */
            builder.Entity<ProductInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.Products)
                .HasForeignKey(z => z.OrderId);

        }
    }
}
