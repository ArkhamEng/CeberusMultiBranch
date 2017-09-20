namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestTrans1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.ProductInventory", "BranchId", "Config.Branch");
            DropForeignKey("Operative.ProductInventory", "ProductId", "Catalog.Product");
            DropIndex("Operative.ProductInventory", new[] { "ProductId" });
            DropIndex("Operative.ProductInventory", new[] { "BranchId" });
            RenameColumn(table: "Operative.Transaction", name: "Folio", newName: "Folio1");
            AddColumn("Operative.Transaction", "IsCompleated", c => c.Boolean(nullable: false));
            AddColumn("Operative.Transaction", "OriginBranchId", c => c.Int());
            AddColumn("Operative.Transaction", "OriginBranch_BranchId", c => c.Int());
            CreateIndex("Operative.Transaction", "OriginBranch_BranchId");
            AddForeignKey("Operative.Transaction", "OriginBranch_BranchId", "Config.Branch", "BranchId");
            DropColumn("Operative.Transaction", "IsFinished");
            DropTable("Operative.ProductInventory");
        }
        
        public override void Down()
        {
            CreateTable(
                "Operative.ProductInventory",
                c => new
                    {
                        ProductInventoryId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Stock = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProductInventoryId);
            
            AddColumn("Operative.Transaction", "IsFinished", c => c.Boolean(nullable: false));
            DropForeignKey("Operative.Transaction", "OriginBranch_BranchId", "Config.Branch");
            DropIndex("Operative.Transaction", new[] { "OriginBranch_BranchId" });
            DropColumn("Operative.Transaction", "OriginBranch_BranchId");
            DropColumn("Operative.Transaction", "OriginBranchId");
            DropColumn("Operative.Transaction", "IsCompleated");
            RenameColumn(table: "Operative.Transaction", name: "Folio1", newName: "Folio");
            CreateIndex("Operative.ProductInventory", "BranchId");
            CreateIndex("Operative.ProductInventory", "ProductId");
            AddForeignKey("Operative.ProductInventory", "ProductId", "Catalog.Product", "ProductId", cascadeDelete: true);
            AddForeignKey("Operative.ProductInventory", "BranchId", "Config.Branch", "BranchId", cascadeDelete: true);
        }
    }
}
