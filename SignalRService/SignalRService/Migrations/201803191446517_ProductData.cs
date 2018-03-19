namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PartNo = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Owner_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserDataModels", t => t.Owner_ID)
                .Index(t => t.Owner_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductModels", "Owner_ID", "dbo.UserDataModels");
            DropIndex("dbo.ProductModels", new[] { "Owner_ID" });
            DropTable("dbo.ProductModels");
        }
    }
}
