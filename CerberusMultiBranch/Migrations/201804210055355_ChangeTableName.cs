namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Operative.CreditNoteChange", newName: "CreditNoteHistory");
        }
        
        public override void Down()
        {
            RenameTable(name: "Operative.CreditNoteHistory", newName: "CreditNoteChange");
        }
    }
}
