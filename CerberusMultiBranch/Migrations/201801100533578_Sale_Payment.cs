namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sale_Payment : DbMigration
    {
        public override void Up()
        {
            RenameColumn("Operative.Payment", "PaymentType", "PaymentMethod");
            RenameColumn("Operative.Sale", "PaymentType", "PaymentMethod");
            
            AddColumn("Operative.Sale", "PaidAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Sale", "DebtAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Sale", "IsCredit", c => c.Boolean(nullable: false));
            
            AddColumn("Operative.Purchase", "PaidAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Purchase", "DebtAmount", c => c.Double(nullable: false));
            AddColumn("Operative.Purchase", "IsCredit", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("Operative.Payment", "PaymentType", c => c.Int(nullable: false));
            AddColumn("Operative.Sale", "PaymentType", c => c.Int(nullable: false));
            DropColumn("Operative.Purchase", "IsCredit");
            DropColumn("Operative.Purchase", "DebtAmount");
            DropColumn("Operative.Purchase", "PaidAmount");
            DropColumn("Operative.Payment", "PaymentMethod");
            DropColumn("Operative.Sale", "IsCredit");
            DropColumn("Operative.Sale", "DebtAmount");
            DropColumn("Operative.Sale", "PaidAmount");
            DropColumn("Operative.Sale", "PaymentMethod");
        }
    }
}
