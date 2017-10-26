namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class externalProductId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Catalog.Equivalence", "ExternalProductId", "Catalog.ExternalProduct");
            DropForeignKey("Catalog.Equivalence", "ProductId", "Catalog.Product");
            DropIndex("Catalog.Equivalence", new[] { "ProductId" });
            DropIndex("Catalog.Equivalence", new[] { "ExternalProductId" });
            AddColumn("Catalog.ExternalProduct", "ProductId", c => c.Int());
            CreateIndex("Catalog.ExternalProduct", "ProductId", name: "IDX_ProductId");
            AddForeignKey("Catalog.ExternalProduct", "ProductId", "Catalog.Product", "ProductId");
            DropTable("Catalog.Equivalence");
        }
        
        public override void Down()
        {
            CreateTable(
                "Catalog.Equivalence",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        ExternalProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.ExternalProductId });
            
            DropForeignKey("Catalog.ExternalProduct", "ProductId", "Catalog.Product");
            DropIndex("Catalog.ExternalProduct", "IDX_ProductId");
            DropColumn("Catalog.ExternalProduct", "ProductId");
            CreateIndex("Catalog.Equivalence", "ExternalProductId");
            CreateIndex("Catalog.Equivalence", "ProductId");
            AddForeignKey("Catalog.Equivalence", "ProductId", "Catalog.Product", "ProductId", cascadeDelete: true);
            AddForeignKey("Catalog.Equivalence", "ExternalProductId", "Catalog.ExternalProduct", "ExternalProductId", cascadeDelete: true);
        }
    }
}
