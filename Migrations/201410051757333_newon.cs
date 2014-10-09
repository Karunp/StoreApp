namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "isSelling", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "isSelling");
        }
    }
}
