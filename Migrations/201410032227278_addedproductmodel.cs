namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedproductmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        StoreId = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Store_StoreId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Stores", t => t.Store_StoreId)
                .Index(t => t.Store_StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Store_StoreId", "dbo.Stores");
            DropIndex("dbo.Products", new[] { "Store_StoreId" });
            DropTable("dbo.Products");
        }
    }
}
