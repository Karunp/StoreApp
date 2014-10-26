namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            AddColumn("dbo.Products", "Category_CategoryId", c => c.Int());
            CreateIndex("dbo.Products", "Category_CategoryId");
            AddForeignKey("dbo.Products", "Category_CategoryId", "dbo.Categories", "CategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Category_CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "Category_CategoryId" });
            DropColumn("dbo.Products", "Category_CategoryId");
            DropTable("dbo.Categories");
        }
    }
}
