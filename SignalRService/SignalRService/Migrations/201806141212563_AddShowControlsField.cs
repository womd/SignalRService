namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShowControlsField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MiningRoomModels", "ShowControls", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MiningRoomModels", "ShowControls");
        }
    }
}
