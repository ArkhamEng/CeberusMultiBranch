namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discountDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("Purchasing.PurchaseItem", "Discount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Purchasing.PurchaseItem", "Discount");
        }
    }
}
