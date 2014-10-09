namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stringid : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", new[] { "User_Id" });
            DropColumn("dbo.Products", "UserId");
            RenameColumn(table: "dbo.Products", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Products", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "UserId" });
            AlterColumn("dbo.Products", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Products", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Products", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "User_Id");
        }
    }
}
