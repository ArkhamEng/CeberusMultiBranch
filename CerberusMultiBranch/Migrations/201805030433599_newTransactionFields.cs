namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTransactionFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Sale", "DiscountedAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Sale", "DiscountPercentage", c => c.Double(nullable: false));
            AddColumn("Operative.Sale", "FinalAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Purchase", "DiscountedAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Purchase", "DiscountPercentage", c => c.Double(nullable: false));
            AddColumn("Operative.Purchase", "FinalAmount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.Purchase", "FinalAmount");
            DropColumn("Operative.Purchase", "DiscountPercentage");
            DropColumn("Operative.Purchase", "DiscountedAmount");
            DropColumn("Operative.Sale", "FinalAmount");
            DropColumn("Operative.Sale", "DiscountPercentage");
            DropColumn("Operative.Sale", "DiscountedAmount");
        }
    }
}
