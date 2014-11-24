namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fuku : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.LocationRatings");
            DropTable("dbo.VoteLogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VoteLogs",
                c => new
                    {
                        AutoId = c.Int(nullable: false, identity: true),
                        SectionId = c.String(),
                        VoteForId = c.String(),
                        UserName = c.String(),
                        Vote = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AutoId);
            
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
    }
}
