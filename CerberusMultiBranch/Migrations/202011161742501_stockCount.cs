namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockCount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inventory.StockCountDetail",
                c => new
                    {
                        StockCountDetailId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UnitCost = c.Double(nullable: false),
                        CurrentQty = c.Double(nullable: false),
                        CountQty = c.Double(nullable: false),
                        VarianceCost = c.Double(nullable: false),
                        VarianceQty = c.Double(nullable: false),
                        Counting_StockCountId = c.Int(),
                    })
                .PrimaryKey(t => t.StockCountDetailId)
                .ForeignKey("Inventory.StockCount", t => t.Counting_StockCountId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.Counting_StockCountId);
            
            CreateTable(
                "Inventory.StockCount",
                c => new
                    {
                        StockCountId = c.Int(nullable: false, identity: true),
                        SystemId = c.Int(nullable: false),
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
                        System_PartSystemId = c.Int(),
                    })
                .PrimaryKey(t => t.StockCountId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Config.PartSystem", t => t.System_PartSystemId)
                .Index(t => new { t.SystemId, t.BranchId, t.Compleation }, name: "IDX_Branch_System_Date")
                .Index(t => t.System_PartSystemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Inventory.StockCountDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Inventory.StockCountDetail", "Counting_StockCountId", "Inventory.StockCount");
            DropForeignKey("Inventory.StockCount", "System_PartSystemId", "Config.PartSystem");
            DropForeignKey("Inventory.StockCount", "BranchId", "Config.Branch");
            DropIndex("Inventory.StockCount", new[] { "System_PartSystemId" });
            DropIndex("Inventory.StockCount", "IDX_Branch_System_Date");
            DropIndex("Inventory.StockCountDetail", new[] { "Counting_StockCountId" });
            DropIndex("Inventory.StockCountDetail", new[] { "ProductId" });
            DropTable("Inventory.StockCount");
            DropTable("Inventory.StockCountDetail");
        }
    }
}
