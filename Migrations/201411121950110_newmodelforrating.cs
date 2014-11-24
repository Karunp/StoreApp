namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmodelforrating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "votes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "votes");
        }
    }
}
