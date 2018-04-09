namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductTMPImport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImportModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductTitle = c.String(),
                        ProductDescription = c.String(),
                        PartNumber = c.String(),
                        Gtin = c.String(),
                        Owner_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDataModels", t => t.Owner_ID, cascadeDelete: true)
                .Index(t => t.Owner_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImportModels", "Owner_ID", "dbo.UserDataModels");
            DropIndex("dbo.ProductImportModels", new[] { "Owner_ID" });
            DropTable("dbo.ProductImportModels");
        }
    }
}
