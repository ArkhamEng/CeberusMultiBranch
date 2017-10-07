namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CashR : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.CashRegister",
                c => new
                    {
                        CashRegisterId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        UserOpen = c.String(),
                        UserClose = c.String(),
                        IsClosed = c.Boolean(nullable: false),
                        InitialAmount = c.Double(nullable: false),
                        FinalAmount = c.Double(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CashRegisterId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .Index(t => t.BranchId);
            
            CreateTable(
                "Operative.CashDetail",
                c => new
                    {
                        CashDetailId = c.Int(nullable: false, identity: true),
                        CashRegisterId = c.Int(nullable: false),
                        User = c.String(),
                        Amount = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CashDetailId)
                .ForeignKey("Operative.CashRegister", t => t.CashRegisterId, cascadeDelete: true)
                .Index(t => t.CashRegisterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.CashDetail", "CashRegisterId", "Operative.CashRegister");
            DropForeignKey("Operative.CashRegister", "BranchId", "Config.Branch");
            DropIndex("Operative.CashDetail", new[] { "CashRegisterId" });
            DropIndex("Operative.CashRegister", new[] { "BranchId" });
            DropTable("Operative.CashDetail");
            DropTable("Operative.CashRegister");
        }
    }
}
