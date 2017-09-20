namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transaction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.Purchase", "BranchId", "Config.Branch");
            DropForeignKey("Operative.Purchase", "EmployeeId", "Catalog.Employee");
            DropForeignKey("Operative.Purchase", "ProviderId", "Catalog.Provider");
            DropForeignKey("Operative.PurchaseDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.PurchaseDetail", "PurchaseId", "Operative.Purchase");
            DropIndex("Operative.Purchase", new[] { "ProviderId" });
            DropIndex("Operative.Purchase", new[] { "BranchId" });
            DropIndex("Operative.Purchase", new[] { "EmployeeId" });
            DropIndex("Operative.PurchaseDetail", new[] { "PurchaseId" });
            DropIndex("Operative.PurchaseDetail", new[] { "ProductId" });
            CreateTable(
                "Operative.Transaction",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        TransactionTypeId = c.Int(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        Bill = c.String(maxLength: 30),
                        TransactionDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IsFinished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .ForeignKey("Security.TransactionTypes", t => t.TransactionTypeId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TransactionTypeId)
                .Index(t => t.ProviderId)
                .Index(t => t.BranchId)
                .Index(t => t.UserId, name: "IDX_UserId");
            
            CreateTable(
                "Operative.TransactionDetail",
                c => new
                    {
                        TransactionDetailId = c.Int(nullable: false, identity: true),
                        TransactionId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionDetailId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Operative.Transaction", t => t.TransactionId, cascadeDelete: true)
                .Index(t => t.TransactionId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Security.TransactionTypes",
                c => new
                    {
                        TransactionTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TransactionTypeId);
            
            DropTable("Operative.Purchase");
            DropTable("Operative.PurchaseDetail");
        }
        
        public override void Down()
        {
            CreateTable(
                "Operative.PurchaseDetail",
                c => new
                    {
                        PurchaseDetailId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        BuyPrice = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseDetailId);
            
            CreateTable(
                "Operative.Purchase",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        ProviderId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        Bill = c.String(nullable: false, maxLength: 30),
                        PurchaseDate = c.DateTime(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId);
            
            DropForeignKey("Operative.Transaction", "UserId", "Security.AspNetUsers");
            DropForeignKey("Operative.Transaction", "TransactionTypeId", "Security.TransactionTypes");
            DropForeignKey("Operative.TransactionDetail", "TransactionId", "Operative.Transaction");
            DropForeignKey("Operative.TransactionDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.Transaction", "ProviderId", "Catalog.Provider");
            DropForeignKey("Operative.Transaction", "BranchId", "Config.Branch");
            DropIndex("Operative.TransactionDetail", new[] { "ProductId" });
            DropIndex("Operative.TransactionDetail", new[] { "TransactionId" });
            DropIndex("Operative.Transaction", "IDX_UserId");
            DropIndex("Operative.Transaction", new[] { "BranchId" });
            DropIndex("Operative.Transaction", new[] { "ProviderId" });
            DropIndex("Operative.Transaction", new[] { "TransactionTypeId" });
            DropTable("Security.TransactionTypes");
            DropTable("Operative.TransactionDetail");
            DropTable("Operative.Transaction");
            CreateIndex("Operative.PurchaseDetail", "ProductId");
            CreateIndex("Operative.PurchaseDetail", "PurchaseId");
            CreateIndex("Operative.Purchase", "EmployeeId");
            CreateIndex("Operative.Purchase", "BranchId");
            CreateIndex("Operative.Purchase", "ProviderId");
            AddForeignKey("Operative.PurchaseDetail", "PurchaseId", "Operative.Purchase", "PurchaseId", cascadeDelete: true);
            AddForeignKey("Operative.PurchaseDetail", "ProductId", "Catalog.Product", "ProductId", cascadeDelete: true);
            AddForeignKey("Operative.Purchase", "ProviderId", "Catalog.Provider", "ProviderId", cascadeDelete: true);
            AddForeignKey("Operative.Purchase", "EmployeeId", "Catalog.Employee", "EmployeeId", cascadeDelete: true);
            AddForeignKey("Operative.Purchase", "BranchId", "Config.Branch", "BranchId", cascadeDelete: true);
        }
    }
}
