namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userproductsid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "UserId");
        }
    }
}
