namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GlobalDiscount : DbMigration
    {
        public override void Up()
        {
            AddColumn("Purchasing.PurchaseOrder", "Insurance", c => c.Double(nullable: false));
            AddColumn("Purchasing.PurchaseOrder", "Discount", c => c.Double(nullable: false));
            AlterColumn("Purchasing.PurchaseOrderHistory", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("Purchasing.PurchaseOrderHistory", "Comment", c => c.String(maxLength: 100));
            DropColumn("Purchasing.PurchaseOrder", "Discount");
            DropColumn("Purchasing.PurchaseOrder", "Insurance");
        }
    }
}
