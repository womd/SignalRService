namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductImportConfigurations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImportConfigurationModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Source = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Owner_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDataModels", t => t.Owner_ID, cascadeDelete: true)
                .Index(t => t.Owner_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImportConfigurationModels", "Owner_ID", "dbo.UserDataModels");
            DropIndex("dbo.ProductImportConfigurationModels", new[] { "Owner_ID" });
            DropTable("dbo.ProductImportConfigurationModels");
        }
    }
}
