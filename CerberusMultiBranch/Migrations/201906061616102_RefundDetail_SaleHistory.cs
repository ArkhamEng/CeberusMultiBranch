namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefundDetail_SaleHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.SaleHistory",
                c => new
                    {
                        SaleHistoryId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        User = c.String(),
                        Comment = c.String(),
                        Status = c.String(),
                        InsDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleHistoryId)
                .ForeignKey("Operative.Sale", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId);
            
            AddColumn("Operative.SaleDetail", "Refund", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.SaleHistory", "SaleId", "Operative.Sale");
            DropIndex("Operative.SaleHistory", new[] { "SaleId" });
            DropColumn("Operative.SaleDetail", "Refund");
            DropTable("Operative.SaleHistory");
        }
    }
}
