namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testingshi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "original_price", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "original_price");
        }
    }
}
