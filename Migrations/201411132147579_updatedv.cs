namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedv : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationRatings",
                c => new
                    {
                        LocationRatingId = c.Int(nullable: false, identity: true),
                        storeid = c.String(),
                        Rating = c.Int(nullable: false),
                        TotalRaters = c.Int(nullable: false),
                        AverageRating = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.LocationRatingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LocationRatings");
        }
    }
}
