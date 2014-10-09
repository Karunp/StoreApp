namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storeid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Store_StoreId", "dbo.Stores");
            DropIndex("dbo.Products", new[] { "Store_StoreId" });
            RenameColumn(table: "dbo.Products", name: "Store_StoreId", newName: "StoreId");
            AlterColumn("dbo.Products", "StoreId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "StoreId");
            AddForeignKey("dbo.Products", "StoreId", "dbo.Stores", "StoreId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "StoreId", "dbo.Stores");
            DropIndex("dbo.Products", new[] { "StoreId" });
            AlterColumn("dbo.Products", "StoreId", c => c.Int());
            RenameColumn(table: "dbo.Products", name: "StoreId", newName: "Store_StoreId");
            CreateIndex("dbo.Products", "Store_StoreId");
            AddForeignKey("dbo.Products", "Store_StoreId", "dbo.Stores", "StoreId");
        }
    }
}
