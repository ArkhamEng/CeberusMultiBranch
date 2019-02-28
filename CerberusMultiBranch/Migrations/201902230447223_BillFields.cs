namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("Purchasing.Purchase", "Freight", c => c.Double(nullable: false));
            AddColumn("Purchasing.Purchase", "Insurance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Purchasing.Purchase", "Insurance");
            DropColumn("Purchasing.Purchase", "Freight");
        }
    }
}
