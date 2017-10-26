namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExternalProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Catalog.Equivalence",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        ExternalProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.ExternalProductId })
                .ForeignKey("Catalog.ExternalProduct", t => t.ExternalProductId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.ExternalProductId);
            
            CreateTable(
                "Catalog.ExternalProduct",
                c => new
                    {
                        ExternalProductId = c.Int(nullable: false, identity: true),
                        ProviderId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 30),
                        Category = c.String(nullable: false, maxLength: 60),
                        Description = c.String(maxLength: 200),
                        Price = c.Double(nullable: false),
                        TradeMark = c.String(maxLength: 50),
                        Unit = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.ExternalProductId)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .Index(t => t.ProviderId)
                .Index(t => t.Code, name: "IDX_Code")
                .Index(t => t.Category, name: "IDX_Category")
                .Index(t => t.Description, name: "IDX_Descripction");
            
        }
        
        public override void Down()
        {
            DropForeignKey("Catalog.Equivalence", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Equivalence", "ExternalProductId", "Catalog.ExternalProduct");
            DropForeignKey("Catalog.ExternalProduct", "ProviderId", "Catalog.Provider");
            DropIndex("Catalog.ExternalProduct", "IDX_Descripction");
            DropIndex("Catalog.ExternalProduct", "IDX_Category");
            DropIndex("Catalog.ExternalProduct", "IDX_Code");
            DropIndex("Catalog.ExternalProduct", new[] { "ProviderId" });
            DropIndex("Catalog.Equivalence", new[] { "ExternalProductId" });
            DropIndex("Catalog.Equivalence", new[] { "ProductId" });
            DropTable("Catalog.ExternalProduct");
            DropTable("Catalog.Equivalence");
        }
    }
}
