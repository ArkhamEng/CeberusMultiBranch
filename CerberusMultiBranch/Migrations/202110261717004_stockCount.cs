namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockCount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inventory.StockCount",
                c => new
                    {
                        StockCountId = c.Int(nullable: false, identity: true),
                        PartSystemId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Description = c.String(),
                        Comment = c.String(),
                        Compleation = c.DateTime(nullable: false),
                        LinesCounted = c.Int(nullable: false),
                        CorrectLines = c.Int(nullable: false),
                        LinesAccurancy = c.Double(nullable: false),
                        TotalCost = c.Double(nullable: false),
                        TotalCostVariance = c.Double(nullable: false),
                        User = c.String(),
                    })
                .PrimaryKey(t => t.StockCountId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Config.PartSystem", t => t.PartSystemId, cascadeDelete: true)
                .Index(t => new { t.PartSystemId, t.BranchId, t.Compleation }, name: "IDX_Branch_System_Date");
            
            CreateTable(
                "Inventory.StockCountDetail",
                c => new
                    {
                        StockCountDetailId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        StockCountId = c.Int(nullable: false),
                        UnitCost = c.Double(nullable: false),
                        CurrentQty = c.Double(nullable: false),
                        CountQty = c.Double(nullable: false),
                        VarianceCost = c.Double(nullable: false),
                        VarianceQty = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.StockCountDetailId)
                .ForeignKey("Inventory.StockCount", t => t.StockCountId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.StockCountId);
            
            AddColumn("Operative.StockMovement", "StockCountId", c => c.Int(nullable:true));
            //AlterColumn("Operative.CreditNoteHistory", "Folio", c => c.String(maxLength: 30));
            CreateIndex("Operative.StockMovement", "StockCountId");
            //CreateIndex("Operative.CreditNoteHistory", new[] { "SaleCreditNoteId", "Folio" });
            AddForeignKey("Operative.StockMovement", "StockCountId", "Inventory.StockCount", "StockCountId");
            //AddForeignKey("Operative.CreditNoteHistory", new[] { "SaleCreditNoteId", "Folio" }, "Operative.SaleCreditNote", new[] { "SaleCreditNoteId", "Folio" });
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.CreditNoteHistory", new[] { "SaleCreditNoteId", "Folio" }, "Operative.SaleCreditNote");
            DropForeignKey("Operative.StockMovement", "StockCountId", "Inventory.StockCount");
            DropForeignKey("Inventory.StockCount", "PartSystemId", "Config.PartSystem");
            DropForeignKey("Inventory.StockCountDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Inventory.StockCountDetail", "StockCountId", "Inventory.StockCount");
            DropForeignKey("Inventory.StockCount", "BranchId", "Config.Branch");
            DropIndex("Operative.CreditNoteHistory", new[] { "SaleCreditNoteId", "Folio" });
            DropIndex("Inventory.StockCountDetail", new[] { "StockCountId" });
            DropIndex("Inventory.StockCountDetail", new[] { "ProductId" });
            DropIndex("Inventory.StockCount", "IDX_Branch_System_Date");
            DropIndex("Operative.StockMovement", new[] { "StockCountId" });
            AlterColumn("Operative.CreditNoteHistory", "Folio", c => c.String());
            DropColumn("Operative.StockMovement", "StockCountId");
            DropTable("Inventory.StockCountDetail");
            DropTable("Inventory.StockCount");
        }
    }
}
