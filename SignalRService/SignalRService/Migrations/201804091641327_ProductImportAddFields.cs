namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductImportAddFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModels", "ImageUrl", c => c.String());
            AddColumn("dbo.ProductModels", "SrcIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductModels", "SrcIdentifier");
            DropColumn("dbo.ProductModels", "ImageUrl");
        }
    }
}
