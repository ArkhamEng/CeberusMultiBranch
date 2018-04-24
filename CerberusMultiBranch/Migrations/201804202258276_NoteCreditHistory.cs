namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoteCreditHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.CreditNoteChange",
                c => new
                    {
                        CreditNoteHistoryId = c.Int(nullable: false, identity: true),
                        SaleCreditNoteId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        User = c.String(maxLength: 30),
                        ChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CreditNoteHistoryId)
                .ForeignKey("Operative.SaleCreditNote", t => t.SaleCreditNoteId, cascadeDelete: true)
                .Index(t => t.SaleCreditNoteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.CreditNoteChange", "SaleCreditNoteId", "Operative.SaleCreditNote");
            DropIndex("Operative.CreditNoteChange", new[] { "SaleCreditNoteId" });
            DropTable("Operative.CreditNoteChange");
        }
    }
}
