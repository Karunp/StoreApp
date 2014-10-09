namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedstuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "url", c => c.String());
            AddColumn("dbo.Products", "image", c => c.String());
            AddColumn("dbo.Products", "title", c => c.String());
            DropColumn("dbo.AspNetUsers", "StoreOwner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "StoreOwner", c => c.Boolean(nullable: false));
            DropColumn("dbo.Products", "title");
            DropColumn("dbo.Products", "image");
            DropColumn("dbo.Products", "url");
        }
    }
}
