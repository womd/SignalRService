namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductTMPImportGoogleFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductImportModels", "SrcId", c => c.String());
            AddColumn("dbo.ProductImportModels", "Title", c => c.String());
            AddColumn("dbo.ProductImportModels", "Description", c => c.String());
            AddColumn("dbo.ProductImportModels", "GoogleProductCategory", c => c.String());
            AddColumn("dbo.ProductImportModels", "ProductType", c => c.String());
            AddColumn("dbo.ProductImportModels", "Link", c => c.String());
            AddColumn("dbo.ProductImportModels", "ImageLink", c => c.String());
            AddColumn("dbo.ProductImportModels", "PriceString", c => c.String());
            AddColumn("dbo.ProductImportModels", "Condition", c => c.String());
            AddColumn("dbo.ProductImportModels", "Availabiliby", c => c.String());
            AddColumn("dbo.ProductImportModels", "Mpn", c => c.String());
            AddColumn("dbo.ProductImportModels", "Brand", c => c.String());
            AddColumn("dbo.ProductImportModels", "CustomLabel0", c => c.String());
            AddColumn("dbo.ProductImportModels", "CustomLabel1", c => c.String());
            AddColumn("dbo.ProductImportModels", "CustomLabel2", c => c.String());
            AddColumn("dbo.ProductImportModels", "CustomLabel3", c => c.String());
            AddColumn("dbo.ProductImportModels", "CustomLabel4", c => c.String());
            AddColumn("dbo.ProductImportModels", "gGuid", c => c.String());
            AddColumn("dbo.ProductImportModels", "IdentifierExists", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductImportModels", "ShippingCountry", c => c.String());
            AddColumn("dbo.ProductImportModels", "ShippingService", c => c.String());
            AddColumn("dbo.ProductImportModels", "ShippingPrice", c => c.String());
            DropColumn("dbo.ProductImportModels", "ProductTitle");
            DropColumn("dbo.ProductImportModels", "ProductDescription");
            DropColumn("dbo.ProductImportModels", "PartNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductImportModels", "PartNumber", c => c.String());
            AddColumn("dbo.ProductImportModels", "ProductDescription", c => c.String());
            AddColumn("dbo.ProductImportModels", "ProductTitle", c => c.String());
            DropColumn("dbo.ProductImportModels", "ShippingPrice");
            DropColumn("dbo.ProductImportModels", "ShippingService");
            DropColumn("dbo.ProductImportModels", "ShippingCountry");
            DropColumn("dbo.ProductImportModels", "IdentifierExists");
            DropColumn("dbo.ProductImportModels", "gGuid");
            DropColumn("dbo.ProductImportModels", "CustomLabel4");
            DropColumn("dbo.ProductImportModels", "CustomLabel3");
            DropColumn("dbo.ProductImportModels", "CustomLabel2");
            DropColumn("dbo.ProductImportModels", "CustomLabel1");
            DropColumn("dbo.ProductImportModels", "CustomLabel0");
            DropColumn("dbo.ProductImportModels", "Brand");
            DropColumn("dbo.ProductImportModels", "Mpn");
            DropColumn("dbo.ProductImportModels", "Availabiliby");
            DropColumn("dbo.ProductImportModels", "Condition");
            DropColumn("dbo.ProductImportModels", "PriceString");
            DropColumn("dbo.ProductImportModels", "ImageLink");
            DropColumn("dbo.ProductImportModels", "Link");
            DropColumn("dbo.ProductImportModels", "ProductType");
            DropColumn("dbo.ProductImportModels", "GoogleProductCategory");
            DropColumn("dbo.ProductImportModels", "Description");
            DropColumn("dbo.ProductImportModels", "Title");
            DropColumn("dbo.ProductImportModels", "SrcId");
        }
    }
}
