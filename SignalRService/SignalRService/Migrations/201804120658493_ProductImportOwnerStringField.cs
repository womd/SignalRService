namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductImportOwnerStringField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductImportModels", "OwnerIdString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductImportModels", "OwnerIdString");
        }
    }
}
