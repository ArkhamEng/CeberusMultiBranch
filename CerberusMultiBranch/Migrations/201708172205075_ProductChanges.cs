namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inventory.ProductFile",
                c => new
                    {
                        ProductFileId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(),
                        File = c.Binary(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.ProductFileId)
                .ForeignKey("Inventory.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Inventory.ProductFile", "ProductId", "Inventory.Product");
            DropIndex("Inventory.ProductFile", new[] { "ProductId" });
            DropTable("Inventory.ProductFile");
        }
    }
}
