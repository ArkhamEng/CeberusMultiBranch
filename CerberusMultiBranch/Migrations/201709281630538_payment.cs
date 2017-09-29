namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.Payment",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        TransactionId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        PaymentType = c.Int(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("Operative.Transaction", t => t.TransactionId, cascadeDelete: true)
                .Index(t => t.TransactionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.Payment", "TransactionId", "Operative.Transaction");
            DropIndex("Operative.Payment", new[] { "TransactionId" });
            DropTable("Operative.Payment");
        }
    }
}
