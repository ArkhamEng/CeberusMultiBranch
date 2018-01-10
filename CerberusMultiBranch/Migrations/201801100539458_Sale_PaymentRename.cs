namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sale_PaymentRename : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Operative.Payment");
            RenameTable(name: "Operative.Payment", newName: "SalePayment");
            MoveTable(name: "Operative.SalePayment", newSchema: "Finances");
            RenameColumn("Finances.SalePayment", "PaymentId", "SalePaymentId");
           // AddColumn("Finances.SalePayment", "SalePaymentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("Finances.SalePayment", "SalePaymentId");
           // DropColumn("Finances.SalePayment", "PaymentId");
        }
        
        public override void Down()
        {
            AddColumn("Finances.SalePayment", "PaymentId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("Finances.SalePayment");
            DropColumn("Finances.SalePayment", "SalePaymentId");
            AddPrimaryKey("Finances.SalePayment", "PaymentId");
            MoveTable(name: "Finances.SalePayment", newSchema: "Operative");
            RenameTable(name: "Operative.SalePayment", newName: "Payment");
        }
    }
}
