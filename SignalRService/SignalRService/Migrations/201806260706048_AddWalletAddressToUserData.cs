namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWalletAddressToUserData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDataModels", "XMRWalletAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDataModels", "XMRWalletAddress");
        }
    }
}
