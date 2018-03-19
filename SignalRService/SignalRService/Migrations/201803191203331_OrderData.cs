namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderModels", "OrderType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderModels", "OrderType");
        }
    }
}
