namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newstorecolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "Prodname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "Prodname");
        }
    }
}
