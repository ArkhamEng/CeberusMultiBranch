namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sale_Payemt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Finances.SalePayment", "SaleId", "Operative.Sale");
            DropIndex("Finances.SalePayment", new[] { "SaleId" });
            CreateTable(
                "Finances.Receivable",
                c => new
                    {
                        ReceivableId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        InitialAmount = c.Double(nullable: false),
                        CurrentAmount = c.Double(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Period = c.Int(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.ReceivableId)
                .ForeignKey("Operative.Sale", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId);
            
            CreateTable(
                "Finances.Payable",
                c => new
                    {
                        PayableId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        InitialAmount = c.Double(nullable: false),
                        CurrentAmount = c.Double(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Period = c.Int(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.PayableId)
                .ForeignKey("Operative.Purchase", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId);
            
            CreateTable(
                "Finances.PurchasePayment",
                c => new
                    {
                        PurchasePaymentId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(),
                        PayableId = c.Int(),
                        Amount = c.Double(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Reference = c.String(maxLength: 30),
                        UpdUser = c.String(maxLength: 100),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchasePaymentId)
                .ForeignKey("Finances.Payable", t => t.PayableId)
                .ForeignKey("Operative.Purchase", t => t.PurchaseId)
                .Index(t => t.PurchaseId)
                .Index(t => t.PayableId);
            
            AddColumn("Operative.BranchProduct", "UpdUser", c => c.DateTime(nullable: false));
            AddColumn("Finances.SalePayment", "ReceivableId", c => c.Int());
            AlterColumn("Finances.SalePayment", "SaleId", c => c.Int());
            CreateIndex("Finances.SalePayment", "SaleId");
            CreateIndex("Finances.SalePayment", "ReceivableId");
            AddForeignKey("Finances.SalePayment", "ReceivableId", "Finances.Receivable", "ReceivableId");
            AddForeignKey("Finances.SalePayment", "SaleId", "Operative.Sale", "SaleId");
            DropColumn("Operative.Sale", "PaidAmount");
            DropColumn("Operative.Sale", "DebtAmount");
            DropColumn("Operative.Purchase", "PaidAmount");
            DropColumn("Operative.Purchase", "DebtAmount");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Purchase", "DebtAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Purchase", "PaidAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Sale", "DebtAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Sale", "PaidAmount", c => c.Double(nullable: false));
            DropForeignKey("Finances.SalePayment", "SaleId", "Operative.Sale");
            DropForeignKey("Finances.PurchasePayment", "PurchaseId", "Operative.Purchase");
            DropForeignKey("Finances.PurchasePayment", "PayableId", "Finances.Payable");
            DropForeignKey("Finances.Payable", "PurchaseId", "Operative.Purchase");
            DropForeignKey("Finances.SalePayment", "ReceivableId", "Finances.Receivable");
            DropForeignKey("Finances.Receivable", "SaleId", "Operative.Sale");
            DropIndex("Finances.PurchasePayment", new[] { "PayableId" });
            DropIndex("Finances.PurchasePayment", new[] { "PurchaseId" });
            DropIndex("Finances.Payable", new[] { "PurchaseId" });
            DropIndex("Finances.Receivable", new[] { "SaleId" });
            DropIndex("Finances.SalePayment", new[] { "ReceivableId" });
            DropIndex("Finances.SalePayment", new[] { "SaleId" });
            AlterColumn("Finances.SalePayment", "SaleId", c => c.Int(nullable: false));
            DropColumn("Finances.SalePayment", "ReceivableId");
            DropColumn("Operative.BranchProduct", "UpdUser");
            DropTable("Finances.PurchasePayment");
            DropTable("Finances.Payable");
            DropTable("Finances.Receivable");
            CreateIndex("Finances.SalePayment", "SaleId");
            AddForeignKey("Finances.SalePayment", "SaleId", "Operative.Sale", "SaleId", cascadeDelete: true);
        }
    }
}
