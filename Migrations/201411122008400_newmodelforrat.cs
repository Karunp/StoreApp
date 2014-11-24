namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmodelforrat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VoteLogs",
                c => new
                    {
                        AutoId = c.Int(nullable: false, identity: true),
                        SectionId = c.String(),
                        VoteForId = c.Int(nullable: false),
                        UserName = c.String(),
                        Vote = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AutoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VoteLogs");
        }
    }
}
