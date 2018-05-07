namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Finances.PurchaseDiscount",
                c => new
                    {
                        PurchaseDiscountId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        DiscountAmount = c.Double(nullable: false),
                        DiscountPercentage = c.Double(nullable: false),
                        Comment = c.String(),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.PurchaseDiscountId)
                .ForeignKey("Operative.Purchase", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Finances.PurchaseDiscount", "PurchaseId", "Operative.Purchase");
            DropIndex("Finances.PurchaseDiscount", new[] { "PurchaseId" });
            DropTable("Finances.PurchaseDiscount");
        }
    }
}
