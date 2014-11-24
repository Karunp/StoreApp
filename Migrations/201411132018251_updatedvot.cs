namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedvot : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VoteLogs", "VoteForId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VoteLogs", "VoteForId", c => c.Int(nullable: false));
        }
    }
}
