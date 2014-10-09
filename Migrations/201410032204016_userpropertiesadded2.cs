namespace StoreOwnerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userpropertiesadded2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Stores", name: "UserId_Id", newName: "User_Id");
            RenameIndex(table: "dbo.Stores", name: "IX_UserId_Id", newName: "IX_User_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Stores", name: "IX_User_Id", newName: "IX_UserId_Id");
            RenameColumn(table: "dbo.Stores", name: "User_Id", newName: "UserId_Id");
        }
    }
}
