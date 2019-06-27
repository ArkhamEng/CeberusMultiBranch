namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditNoteChangeRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.CreditNoteHistory","FK_Operative.CreditNoteChange_Operative.SaleCreditNote_SaleCreditNoteId");
            DropForeignKey("Operative.SaleCreditNote", "SaleCreditNoteId", "Operative.Sale");
            DropIndex("Operative.SaleCreditNote", "IDX_Identifier_Active");
            DropPrimaryKey("Operative.SaleCreditNote");
            AddColumn("Operative.CreditNoteHistory", "Folio", c => c.String());
            AlterColumn("Operative.SaleCreditNote", "Folio", c => c.String(nullable: false, maxLength: 30));
            AddPrimaryKey("Operative.SaleCreditNote", new[] { "SaleCreditNoteId", "Folio" });
            CreateIndex("Operative.SaleCreditNote", new[] { "Folio", "Ident", "IsActive" }, name: "IDX_Identifier_Active");
            AddForeignKey("Operative.SaleCreditNote", "SaleCreditNoteId", "Operative.Sale", "SaleId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.SaleCreditNote", "SaleCreditNoteId", "Operative.Sale");
            DropIndex("Operative.SaleCreditNote", "IDX_Identifier_Active");
            DropPrimaryKey("Operative.SaleCreditNote");
            AlterColumn("Operative.SaleCreditNote", "Folio", c => c.String(maxLength: 30));
            DropColumn("Operative.CreditNoteHistory", "Folio");
            AddPrimaryKey("Operative.SaleCreditNote", "SaleCreditNoteId");
            CreateIndex("Operative.SaleCreditNote", new[] { "Folio", "Ident", "IsActive" }, name: "IDX_Identifier_Active");
            AddForeignKey("Operative.SaleCreditNote", "SaleCreditNoteId", "Operative.Sale", "SaleId");
        }
    }
}
