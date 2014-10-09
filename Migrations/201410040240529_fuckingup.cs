namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fuckingup : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "StoreId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "StoreId", c => c.String());
        }
    }
}
