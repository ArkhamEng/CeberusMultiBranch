namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Purchase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.Purchase",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        ProviderId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        Bill = c.String(maxLength: 30),
                        PurchaseDate = c.String(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Employee", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: false)
                .Index(t => t.ProviderId)
                .Index(t => t.BranchId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "Operative.PurchaseDetail",
                c => new
                    {
                        PurchaseDetailId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        BuyPrice = c.Double(nullable: false),
                        Active = c.Boolean(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseDetailId)
                .ForeignKey("Operative.Purchase", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.PurchaseDetail", "PurchaseId", "Operative.Purchase");
            DropForeignKey("Operative.Purchase", "ProviderId", "Catalog.Provider");
            DropForeignKey("Operative.Purchase", "EmployeeId", "Catalog.Employee");
            DropForeignKey("Operative.Purchase", "BranchId", "Config.Branch");
            DropIndex("Operative.PurchaseDetail", new[] { "PurchaseId" });
            DropIndex("Operative.Purchase", new[] { "EmployeeId" });
            DropIndex("Operative.Purchase", new[] { "BranchId" });
            DropIndex("Operative.Purchase", new[] { "ProviderId" });
            DropTable("Operative.PurchaseDetail");
            DropTable("Operative.Purchase");
        }
    }
}
