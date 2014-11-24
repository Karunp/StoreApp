namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedvote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "storenum", c => c.String());
            AddColumn("dbo.Stores", "Votes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "Votes");
            DropColumn("dbo.Stores", "storenum");
        }
    }
}
