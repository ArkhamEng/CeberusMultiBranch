namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseOrderImprovement : DbMigration
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
            
            CreateTable(
                "Purchasing.PurchaseOrderHistory",
                c => new
                    {
                        PurchaseOrderHistoryId = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Status = c.String(maxLength: 50),
                        Type = c.String(maxLength: 50),
                        ShipMethod = c.String(maxLength: 50),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ModifyByUser = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderHistoryId)
                .ForeignKey("Purchasing.PurchaseOrder", t => t.PurchaseOrderId, cascadeDelete: true)
                .Index(t => t.PurchaseOrderId);
            
            AddColumn("Catalog.Product", "IsTrackable", c => c.Boolean(nullable: false));
            AddColumn("Catalog.Equivalence", "BuyPrice", c => c.Double(nullable: false));
            AddColumn("Catalog.Equivalence", "IsDefault", c => c.Boolean(nullable: false));
            AddColumn("Catalog.Equivalence", "InsDate", c => c.DateTime(nullable: false));
            AddColumn("Catalog.Equivalence", "InsUser", c => c.String());
            AddColumn("Catalog.Equivalence", "UpdDate", c => c.DateTime(nullable: false));
            AddColumn("Catalog.Equivalence", "UpdUser", c => c.String());
            AddColumn("Purchasing.PurchaseItem", "Comment", c => c.String());
            AddColumn("Purchasing.PurchaseItem", "DaysToPay", c => c.Int(nullable: false));
            AddColumn("Purchasing.PurchaseItem", "TaxRate", c => c.Double(nullable: false));
            AddColumn("Purchasing.PurchaseOrder", "Bill", c => c.String());
            AddColumn("Purchasing.PurchaseOrder", "DaysToPay", c => c.Int(nullable: false));
            AddColumn("Purchasing.PurchaseOrder", "Freight", c => c.Double(nullable: false));
            AddColumn("Purchasing.PurchaseOrder", "Comment", c => c.String());
            AddColumn("Purchasing.PurchaseOrder", "Year", c => c.Int(nullable: false));
            AddColumn("Purchasing.PurchaseOrder", "Sequential", c => c.Int(nullable: false));
            AddColumn("Purchasing.PurchaseOrderDetail", "StockedQty", c => c.Double(nullable: false));
            AddColumn("Purchasing.PurchaseOrderDetail", "Comment", c => c.String());
            AddColumn("Purchasing.PurchaseOrderDetail", "IsCompleated", c => c.Boolean(nullable: false));
            AddColumn("Purchasing.PurchaseOrderDetail", "Discount", c => c.Double(nullable: false));
            AddColumn("Operative.StockMovement", "TrackingItemId", c => c.Int());
            AddColumn("Operative.StockMovement", "PurchaseOrderDetailId", c => c.Int());
            AlterColumn("Purchasing.PurchaseOrder", "OrderDate", c => c.DateTime());
            AlterColumn("Purchasing.PurchaseOrder", "DueDate", c => c.DateTime());
            AlterColumn("Purchasing.PurchaseOrder", "ShipDate", c => c.DateTime());
            AlterColumn("Purchasing.PurchaseOrder", "DeliveryDate", c => c.DateTime());
            AlterColumn("Config.Variable", "Value", c => c.String());
            CreateIndex("Operative.StockMovement", "TrackingItemId");
            CreateIndex("Operative.StockMovement", "PurchaseOrderDetailId");
            AddForeignKey("Catalog.Equivalence", "ProviderId", "Catalog.Provider", "ProviderId", cascadeDelete: true);
            AddForeignKey("Operative.StockMovement", "PurchaseOrderDetailId", "Purchasing.PurchaseOrderDetail", "PurchaseOrderDetailId");
            AddForeignKey("Operative.StockMovement", "TrackingItemId", "Operative.TrackingItem", "TrackingItemId");
            DropColumn("Purchasing.PurchaseOrderDetail", "StokedQty");
        }
        
        public override void Down()
        {
            AddColumn("Purchasing.PurchaseOrderDetail", "StokedQty", c => c.Double(nullable: false));
            DropForeignKey("Operative.InventoryItem", "TrackingItemId", "Operative.TrackingItem");
            DropForeignKey("Operative.TrackingItem", "ProductId", "Catalog.Product");
            DropForeignKey("Purchasing.PurchaseOrderHistory", "PurchaseOrderId", "Purchasing.PurchaseOrder");
            DropForeignKey("Operative.StockMovement", "TrackingItemId", "Operative.TrackingItem");
            DropForeignKey("Operative.StockMovement", "PurchaseOrderDetailId", "Purchasing.PurchaseOrderDetail");
            DropForeignKey("Catalog.Equivalence", "ProviderId", "Catalog.Provider");
            DropForeignKey("Operative.InventoryItem", new[] { "BranchId", "ProductId" }, "Operative.BranchProduct");
            DropIndex("Purchasing.PurchaseOrderHistory", new[] { "PurchaseOrderId" });
            DropIndex("Operative.StockMovement", new[] { "PurchaseOrderDetailId" });
            DropIndex("Operative.StockMovement", new[] { "TrackingItemId" });
            DropIndex("Operative.TrackingItem", new[] { "ProductId" });
            DropIndex("Operative.InventoryItem", new[] { "TrackingItemId" });
            DropIndex("Operative.InventoryItem", new[] { "BranchId", "ProductId" });
            AlterColumn("Config.Variable", "Value", c => c.String(maxLength: 15));
            AlterColumn("Purchasing.PurchaseOrder", "DeliveryDate", c => c.DateTime(nullable: false));
            AlterColumn("Purchasing.PurchaseOrder", "ShipDate", c => c.DateTime(nullable: false));
            AlterColumn("Purchasing.PurchaseOrder", "DueDate", c => c.DateTime(nullable: false));
            AlterColumn("Purchasing.PurchaseOrder", "OrderDate", c => c.DateTime(nullable: false));
            DropColumn("Operative.StockMovement", "PurchaseOrderDetailId");
            DropColumn("Operative.StockMovement", "TrackingItemId");
            DropColumn("Purchasing.PurchaseOrderDetail", "Discount");
            DropColumn("Purchasing.PurchaseOrderDetail", "IsCompleated");
            DropColumn("Purchasing.PurchaseOrderDetail", "Comment");
            DropColumn("Purchasing.PurchaseOrderDetail", "StockedQty");
            DropColumn("Purchasing.PurchaseOrder", "Sequential");
            DropColumn("Purchasing.PurchaseOrder", "Year");
            DropColumn("Purchasing.PurchaseOrder", "Comment");
            DropColumn("Purchasing.PurchaseOrder", "Freight");
            DropColumn("Purchasing.PurchaseOrder", "DaysToPay");
            DropColumn("Purchasing.PurchaseOrder", "Bill");
            DropColumn("Purchasing.PurchaseItem", "TaxRate");
            DropColumn("Purchasing.PurchaseItem", "DaysToPay");
            DropColumn("Purchasing.PurchaseItem", "Comment");
            DropColumn("Catalog.Equivalence", "UpdUser");
            DropColumn("Catalog.Equivalence", "UpdDate");
            DropColumn("Catalog.Equivalence", "InsUser");
            DropColumn("Catalog.Equivalence", "InsDate");
            DropColumn("Catalog.Equivalence", "IsDefault");
            DropColumn("Catalog.Equivalence", "BuyPrice");
            DropColumn("Catalog.Product", "IsTrackable");
            DropTable("Purchasing.PurchaseOrderHistory");
            DropTable("Operative.TrackingItem");
            DropTable("Operative.InventoryItem");
        }
    }
}
