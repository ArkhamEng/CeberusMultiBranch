namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Taxes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Sale", "TotalTaxedAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Sale", "TotalTaxAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Purchase", "TotalTaxedAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Purchase", "TotalTaxAmount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.Purchase", "TotalTaxAmount");
            DropColumn("Operative.Purchase", "TotalTaxedAmount");
            DropColumn("Operative.Sale", "TotalTaxAmount");
            DropColumn("Operative.Sale", "TotalTaxedAmount");
        }
    }
}
