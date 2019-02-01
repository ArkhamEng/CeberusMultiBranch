namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tracking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.InventoryItem",
                c => new
                    {
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        TrackingItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BranchId, t.ProductId, t.TrackingItemId })
                .ForeignKey("Operative.BranchProduct", t => new { t.BranchId, t.ProductId }, cascadeDelete: true)
                .ForeignKey("Operative.TrackingItem", t => t.TrackingItemId, cascadeDelete: true)
                .Index(t => new { t.BranchId, t.ProductId })
                .Index(t => t.TrackingItemId);
            
            CreateTable(
                "Operative.TrackingItem",
                c => new
                    {
                        TrackingItemId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        SerialNumber = c.String(),
                        IsBatch = c.Boolean(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                    })
                .PrimaryKey(t => t.TrackingItemId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: false)
                .Index(t => t.ProductId);
            
            AddColumn("Purchasing.PurchaseItem", "Comment", c => c.String());
            AddColumn("Purchasing.PurchaseItem", "DaysToPay", c => c.Int(nullable: false));
            AddColumn("Purchasing.PurchaseItem", "TaxRate", c => c.Double(nullable: false));
            AddColumn("Purchasing.PurchaseOrder", "Bill", c => c.String());
            AddColumn("Purchasing.PurchaseOrder", "DaysToPay", c => c.Int(nullable: false));
            AddColumn("Purchasing.PurchaseOrder", "Freight", c => c.Double(nullable: false));
            AddColumn("Purchasing.PurchaseOrderDetail", "StockedQty", c => c.Double(nullable: false));
            AddColumn("Purchasing.PurchaseOrderDetail", "Comment", c => c.String());
            AddColumn("Purchasing.PurchaseOrderDetail", "IsCompleated", c => c.Boolean(nullable: false));
            AddColumn("Purchasing.PurchaseOrderDetail", "Discount", c => c.Double(nullable: false));
            AddColumn("Operative.StockMovement", "TrackingItemId", c => c.Int());
            AddColumn("Operative.StockMovement", "PurchaseOrderDetailId", c => c.Int());
            CreateIndex("Operative.StockMovement", "TrackingItemId");
            CreateIndex("Operative.StockMovement", "PurchaseOrderDetailId");
            AddForeignKey("Operative.StockMovement", "PurchaseOrderDetailId", "Purchasing.PurchaseOrderDetail", "PurchaseOrderDetailId");
            AddForeignKey("Operative.StockMovement", "TrackingItemId", "Operative.TrackingItem", "TrackingItemId");
            DropColumn("Purchasing.PurchaseOrderDetail", "StokedQty");
        }
        
        public override void Down()
        {
            AddColumn("Purchasing.PurchaseOrderDetail", "StokedQty", c => c.Double(nullable: false));
            DropForeignKey("Operative.InventoryItem", "TrackingItemId", "Operative.TrackingItem");
            DropForeignKey("Operative.TrackingItem", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.StockMovement", "TrackingItemId", "Operative.TrackingItem");
            DropForeignKey("Operative.StockMovement", "PurchaseOrderDetailId", "Purchasing.PurchaseOrderDetail");
            DropForeignKey("Operative.InventoryItem", new[] { "BranchId", "ProductId" }, "Operative.BranchProduct");
            DropIndex("Operative.StockMovement", new[] { "PurchaseOrderDetailId" });
            DropIndex("Operative.StockMovement", new[] { "TrackingItemId" });
            DropIndex("Operative.TrackingItem", new[] { "ProductId" });
            DropIndex("Operative.InventoryItem", new[] { "TrackingItemId" });
            DropIndex("Operative.InventoryItem", new[] { "BranchId", "ProductId" });
            DropColumn("Operative.StockMovement", "PurchaseOrderDetailId");
            DropColumn("Operative.StockMovement", "TrackingItemId");
            DropColumn("Purchasing.PurchaseOrderDetail", "Discount");
            DropColumn("Purchasing.PurchaseOrderDetail", "IsCompleated");
            DropColumn("Purchasing.PurchaseOrderDetail", "Comment");
            DropColumn("Purchasing.PurchaseOrderDetail", "StockedQty");
            DropColumn("Purchasing.PurchaseOrder", "Freight");
            DropColumn("Purchasing.PurchaseOrder", "DaysToPay");
            DropColumn("Purchasing.PurchaseOrder", "Bill");
            DropColumn("Purchasing.PurchaseItem", "TaxRate");
            DropColumn("Purchasing.PurchaseItem", "DaysToPay");
            DropColumn("Purchasing.PurchaseItem", "Comment");
            DropTable("Operative.TrackingItem");
            DropTable("Operative.InventoryItem");
        }
    }
}
