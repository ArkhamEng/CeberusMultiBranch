namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentCreditNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.SaleCreditNote", "Ident", c => c.String(maxLength: 15));
            CreateIndex("Operative.SaleCreditNote", "Ident", name: "IDX_Ident");
        }
        
        public override void Down()
        {
            DropIndex("Operative.SaleCreditNote", "IDX_Ident");
            DropColumn("Operative.SaleCreditNote", "Ident");
        }
    }
}
