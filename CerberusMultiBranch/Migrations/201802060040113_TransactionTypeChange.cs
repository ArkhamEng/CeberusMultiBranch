namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionTypeChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Sale", "TransactionType", c => c.Int(nullable: false));
            AddColumn("Operative.Purchase", "TransactionType", c => c.Int(nullable: false));
            DropColumn("Operative.Sale", "IsCredit");
            DropColumn("Operative.Purchase", "IsCredit");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Purchase", "IsCredit", c => c.Boolean(nullable: false));
            AddColumn("Operative.Sale", "IsCredit", c => c.Boolean(nullable: false));
            DropColumn("Operative.Purchase", "TransactionType");
            DropColumn("Operative.Sale", "TransactionType");
        }
    }
}
