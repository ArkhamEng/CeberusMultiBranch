namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Purchase2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.PurchaseDetail", "Amount", c => c.Double(nullable: false));
            DropColumn("Operative.PurchaseDetail", "Amout");
            DropColumn("Operative.PurchaseDetail", "Unit");
            DropColumn("Operative.PurchaseDetail", "Active");
        }
        
        public override void Down()
        {
            AddColumn("Operative.PurchaseDetail", "Active", c => c.Boolean(nullable: false));
            AddColumn("Operative.PurchaseDetail", "Unit", c => c.String(maxLength: 29));
            AddColumn("Operative.PurchaseDetail", "Amout", c => c.Double(nullable: false));
            DropColumn("Operative.PurchaseDetail", "Amount");
        }
    }
}
