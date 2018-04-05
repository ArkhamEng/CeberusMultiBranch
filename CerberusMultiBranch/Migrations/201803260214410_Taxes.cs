namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Taxes : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.SaleDetail", "TaxPercentage", c => c.Double(nullable: false));
            AddColumn("Operative.SaleDetail", "TaxAmount", c => c.Double(nullable: false));
            AddColumn("Operative.SaleDetail", "TaxedPrice", c => c.Double(nullable: false));
            AddColumn("Operative.SaleDetail", "TaxedAmount", c => c.Double(nullable: false));
            AddColumn("Operative.PurchaseDetail", "TaxPercentage", c => c.Double(nullable: false));
            AddColumn("Operative.PurchaseDetail", "TaxAmount", c => c.Double(nullable: false));
            AddColumn("Operative.PurchaseDetail", "TaxedPrice", c => c.Double(nullable: false));
            AddColumn("Operative.PurchaseDetail", "TaxedAmount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.PurchaseDetail", "TaxedAmount");
            DropColumn("Operative.PurchaseDetail", "TaxedPrice");
            DropColumn("Operative.PurchaseDetail", "TaxAmount");
            DropColumn("Operative.PurchaseDetail", "TaxPercentage");
            DropColumn("Operative.SaleDetail", "TaxedAmount");
            DropColumn("Operative.SaleDetail", "TaxedPrice");
            DropColumn("Operative.SaleDetail", "TaxAmount");
            DropColumn("Operative.SaleDetail", "TaxPercentage");
        }
    }
}
