namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.PurchaseDetail", "Amout", c => c.Double(nullable: false));
            AddColumn("Operative.PurchaseDetail", "Unit", c => c.String(maxLength: 29));
            DropColumn("Operative.PurchaseDetail", "EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("Operative.PurchaseDetail", "EmployeeId", c => c.Int(nullable: false));
            DropColumn("Operative.PurchaseDetail", "Unit");
            DropColumn("Operative.PurchaseDetail", "Amout");
        }
    }
}
