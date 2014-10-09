namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelupdates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        lat = c.String(),
                        lng = c.String(),
                        name = c.String(),
                        city = c.String(),
                        address = c.String(),
                        phone = c.String(),
                        state = c.String(),
                        store_id = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Locations");
        }
    }
}
