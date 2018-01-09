namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.Purchase",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        ProviderId = c.Int(nullable: false),
                        Bill = c.String(nullable: false, maxLength: 30),
                        Expiration = c.DateTime(nullable: false),
                        BranchId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        TotalAmount = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        LastStatus = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        TransactionDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProviderId)
                .Index(t => t.BranchId, name: "IDX_BranchId")
                .Index(t => t.UserId, name: "IDX_UserId")
                .Index(t => t.Status, name: "IDX_Status")
                .Index(t => t.TransactionDate, name: "IDX_TransactionDate");
            
            CreateTable(
                "Operative.PurchaseDetail",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseId, t.ProductId })
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Operative.Purchase", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.Purchase", "UserId", "Security.AspNetUsers");
            DropForeignKey("Operative.PurchaseDetail", "PurchaseId", "Operative.Purchase");
            DropForeignKey("Operative.PurchaseDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.Purchase", "ProviderId", "Catalog.Provider");
            DropForeignKey("Operative.Purchase", "BranchId", "Config.Branch");
            DropIndex("Operative.PurchaseDetail", new[] { "ProductId" });
            DropIndex("Operative.PurchaseDetail", new[] { "PurchaseId" });
            DropIndex("Operative.Purchase", "IDX_TransactionDate");
            DropIndex("Operative.Purchase", "IDX_Status");
            DropIndex("Operative.Purchase", "IDX_UserId");
            DropIndex("Operative.Purchase", "IDX_BranchId");
            DropIndex("Operative.Purchase", new[] { "ProviderId" });
            DropTable("Operative.PurchaseDetail");
            DropTable("Operative.Purchase");
        }
    }
}
