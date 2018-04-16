namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refunding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.SaleCreditNote",
                c => new
                    {
                        SaleCreditNoteId = c.Int(nullable: false),
                        Folio = c.String(maxLength: 30),
                        Amount = c.Double(nullable: false),
                        Year = c.Int(nullable: false),
                        Sequential = c.Int(nullable: false),
                        User = c.String(maxLength: 30),
                        RegisterDate = c.DateTime(nullable: false),
                        ExplirationDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SaleCreditNoteId)
                .ForeignKey("Operative.Sale", t => t.SaleCreditNoteId)
                .Index(t => t.SaleCreditNoteId)
                .Index(t => t.Year, name: "IDX_Year")
                .Index(t => t.Sequential, name: "IDX_Sequential");
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.SaleCreditNote", "SaleCreditNoteId", "Operative.Sale");
            DropIndex("Operative.SaleCreditNote", "IDX_Sequential");
            DropIndex("Operative.SaleCreditNote", "IDX_Year");
            DropIndex("Operative.SaleCreditNote", new[] { "SaleCreditNoteId" });
            DropTable("Operative.SaleCreditNote");
        }
    }
}
