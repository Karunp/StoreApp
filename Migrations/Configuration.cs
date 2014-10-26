namespace StoreOwnerApp.Migrations
{
    using StoreOwnerApp.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StoreOwnerApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void Seed(StoreOwnerApp.Models.ApplicationDbContext context)
        {
             //public DbSet<Product> Products { get; set; }
            

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
