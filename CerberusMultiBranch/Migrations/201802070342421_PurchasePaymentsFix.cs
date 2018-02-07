namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchasePaymentsFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Finances.Payable", "PurchaseId", "Operative.Purchase");
            DropForeignKey("Finances.PurchasePayment", "PayableId", "Finances.Payable");
            DropForeignKey("Finances.PurchasePayment", "PurchaseId", "Operative.Purchase");
            DropIndex("Finances.Payable", new[] { "PurchaseId" });
            DropIndex("Finances.PurchasePayment", new[] { "PurchaseId" });
            DropIndex("Finances.PurchasePayment", new[] { "PayableId" });
            AlterColumn("Finances.PurchasePayment", "PurchaseId", c => c.Int(nullable: false));
            CreateIndex("Finances.PurchasePayment", "PurchaseId");
            AddForeignKey("Finances.PurchasePayment", "PurchaseId", "Operative.Purchase", "PurchaseId", cascadeDelete: true);
            DropColumn("Finances.PurchasePayment", "PayableId");
            DropTable("Finances.Payable");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.PayableId);
            
            AddColumn("Finances.PurchasePayment", "PayableId", c => c.Int());
            DropForeignKey("Finances.PurchasePayment", "PurchaseId", "Operative.Purchase");
            DropIndex("Finances.PurchasePayment", new[] { "PurchaseId" });
            AlterColumn("Finances.PurchasePayment", "PurchaseId", c => c.Int());
            CreateIndex("Finances.PurchasePayment", "PayableId");
            CreateIndex("Finances.PurchasePayment", "PurchaseId");
            CreateIndex("Finances.Payable", "PurchaseId");
            AddForeignKey("Finances.PurchasePayment", "PurchaseId", "Operative.Purchase", "PurchaseId");
            AddForeignKey("Finances.PurchasePayment", "PayableId", "Finances.Payable", "PayableId");
            AddForeignKey("Finances.Payable", "PurchaseId", "Operative.Purchase", "PurchaseId", cascadeDelete: true);
        }
    }
}
