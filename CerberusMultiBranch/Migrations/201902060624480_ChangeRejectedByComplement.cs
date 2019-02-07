namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRejectedByComplement : DbMigration
    {
        public override void Up()
        {
            AddColumn("Purchasing.PurchaseOrderDetail", "ComplementQty", c => c.Double(nullable: false));
            DropColumn("Purchasing.PurchaseOrderDetail", "RejectedQty");
        }
        
        public override void Down()
        {
            AddColumn("Purchasing.PurchaseOrderDetail", "RejectedQty", c => c.Double(nullable: false));
            DropColumn("Purchasing.PurchaseOrderDetail", "ComplementQty");
        }
    }
}
