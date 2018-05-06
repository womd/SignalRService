namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameTotalAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDataModels", "TotalMoney", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDataModels", "TotalMoney");
        }
    }
}
