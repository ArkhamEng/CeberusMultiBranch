namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditNoteHistoryRemoveDependecies : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.CreditNoteHistory", "SaleCreditNoteId", "Operative.SaleCreditNote");
            DropIndex("Operative.CreditNoteHistory", new[] { "SaleCreditNoteId" });
        }
        
        public override void Down()
        {
            CreateIndex("Operative.CreditNoteHistory", "SaleCreditNoteId");
            AddForeignKey("Operative.CreditNoteHistory", "SaleCreditNoteId", "Operative.SaleCreditNote", "SaleCreditNoteId", cascadeDelete: true);
        }
    }
}
