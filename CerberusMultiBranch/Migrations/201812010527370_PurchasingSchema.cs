namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchasingSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Purchasing.PurchaseItem",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        PurchaseTypeId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        TotalLine = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.BranchId, t.ProductId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: false)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: false)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: false)
                .ForeignKey("Purchasing.PurchaseType", t => t.PurchaseTypeId, cascadeDelete: false)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.BranchId)
                .Index(t => t.ProductId)
                .Index(t => t.ProviderId)
                .Index(t => t.PurchaseTypeId);
            
            CreateTable(
                "Purchasing.PurchaseType",
                c => new
                    {
                        PurchaseTypeId = c.Int(nullable: false),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseTypeId);
            
            CreateTable(
                "Purchasing.PurchaseOrder",
                c => new
                    {
                        PurchaseOrderId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        Folio = c.String(),
                        PurchaseStatusId = c.Int(nullable: false),
                        PurchaseTypeId = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ShipDate = c.DateTime(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        SubTotal = c.Double(nullable: false),
                        TaxRate = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseOrderId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: false)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: false)
                .ForeignKey("Purchasing.PurchaseStatus", t => t.PurchaseStatusId, cascadeDelete: false)
                .ForeignKey("Purchasing.PurchaseType", t => t.PurchaseTypeId, cascadeDelete: false)
                .ForeignKey("Catalog.ShipMethod", t => t.ShipMethodId, cascadeDelete: false)
                .Index(t => t.BranchId)
                .Index(t => t.ProviderId)
                .Index(t => t.PurchaseStatusId)
                .Index(t => t.PurchaseTypeId)
                .Index(t => t.ShipMethodId);
            
            CreateTable(
                "Purchasing.PurchaseOrderDetail",
                c => new
                    {
                        PurchaseOrderDetailId = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        OrderQty = c.Double(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        LineTotal = c.Double(nullable: false),
                        ReceivedQty = c.Double(nullable: false),
                        RejectedQty = c.Double(nullable: false),
                        StokedQty = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseOrderDetailId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: false)
                .ForeignKey("Purchasing.PurchaseOrder", t => t.PurchaseOrderId, cascadeDelete: false)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Purchasing.PurchaseStatus",
                c => new
                    {
                        PurchaseStatusId = c.Int(nullable: false),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseStatusId);
            
            CreateTable(
                "Catalog.ShipMethod",
                c => new
                    {
                        ShipMethodId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 50),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.ShipMethodId);
            
            AddColumn("Operative.Purchase", "Folio", c => c.String(maxLength: 20));
            AddColumn("Operative.PurchaseDetail", "BranchId", c => c.Int());
            AddColumn("Operative.PurchaseDetail", "Received", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Operative.PurchaseDetail", "Stocked", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Operative.PurchaseDetail", "Rejected", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("Operative.PurchaseDetail", "BranchId");
            AddForeignKey("Operative.PurchaseDetail", "BranchId", "Config.Branch", "BranchId");
        }
        
        public override void Down()
        {
            DropForeignKey("Purchasing.PurchaseItem", "UserId", "Security.AspNetUsers");
            DropForeignKey("Purchasing.PurchaseOrder", "ShipMethodId", "Catalog.ShipMethod");
            DropForeignKey("Purchasing.PurchaseOrder", "PurchaseTypeId", "Purchasing.PurchaseType");
            DropForeignKey("Purchasing.PurchaseOrder", "PurchaseStatusId", "Purchasing.PurchaseStatus");
            DropForeignKey("Purchasing.PurchaseOrderDetail", "PurchaseOrderId", "Purchasing.PurchaseOrder");
            DropForeignKey("Purchasing.PurchaseOrderDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Purchasing.PurchaseOrder", "ProviderId", "Catalog.Provider");
            DropForeignKey("Purchasing.PurchaseOrder", "BranchId", "Config.Branch");
            DropForeignKey("Purchasing.PurchaseItem", "PurchaseTypeId", "Purchasing.PurchaseType");
            DropForeignKey("Purchasing.PurchaseItem", "ProviderId", "Catalog.Provider");
            DropForeignKey("Purchasing.PurchaseItem", "ProductId", "Catalog.Product");
            DropForeignKey("Purchasing.PurchaseItem", "BranchId", "Config.Branch");
            DropForeignKey("Operative.PurchaseDetail", "BranchId", "Config.Branch");
            DropIndex("Purchasing.PurchaseOrderDetail", new[] { "ProductId" });
            DropIndex("Purchasing.PurchaseOrderDetail", new[] { "PurchaseOrderId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "ShipMethodId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "PurchaseTypeId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "PurchaseStatusId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "ProviderId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "BranchId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "PurchaseTypeId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "ProviderId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "ProductId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "BranchId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "UserId" });
            DropIndex("Operative.PurchaseDetail", new[] { "BranchId" });
            DropColumn("Operative.PurchaseDetail", "Rejected");
            DropColumn("Operative.PurchaseDetail", "Stocked");
            DropColumn("Operative.PurchaseDetail", "Received");
            DropColumn("Operative.PurchaseDetail", "BranchId");
            DropColumn("Operative.Purchase", "Folio");
            DropTable("Catalog.ShipMethod");
            DropTable("Purchasing.PurchaseStatus");
            DropTable("Purchasing.PurchaseOrderDetail");
            DropTable("Purchasing.PurchaseOrder");
            DropTable("Purchasing.PurchaseType");
            DropTable("Purchasing.PurchaseItem");
        }
    }
}
