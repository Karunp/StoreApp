namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedvo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.Locations", "TotalRaters", c => c.Int(nullable: false));
            DropColumn("dbo.Locations", "votes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "votes", c => c.String());
            DropColumn("dbo.Locations", "TotalRaters");
            DropColumn("dbo.Locations", "Rating");
        }
    }
}
