namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionSchema : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Security.TransactionTypes", newName: "TransactionType");
            MoveTable(name: "Security.TransactionType", newSchema: "Operative");
        }
        
        public override void Down()
        {
            MoveTable(name: "Operative.TransactionType", newSchema: "Security");
            RenameTable(name: "Security.TransactionType", newName: "TransactionTypes");
        }
    }
}
