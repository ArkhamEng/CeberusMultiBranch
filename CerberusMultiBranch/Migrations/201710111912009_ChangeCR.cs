namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCR : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.CashRegister", "OpeningDate", c => c.DateTime(nullable: false));
            AddColumn("Operative.CashRegister", "ClosingDate", c => c.DateTime());
            AlterColumn("Operative.CashRegister", "UserOpen", c => c.String(maxLength: 50));
            AlterColumn("Operative.CashRegister", "UserClose", c => c.String(maxLength: 50));
            DropColumn("Operative.CashRegister", "IsClosed");
            DropColumn("Operative.CashRegister", "BeginDate");
            DropColumn("Operative.CashRegister", "EndDate");
            DropColumn("Operative.CashRegister", "ShiftNumber");
        }
        
        public override void Down()
        {
            AddColumn("Operative.CashRegister", "ShiftNumber", c => c.Int(nullable: false));
            AddColumn("Operative.CashRegister", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("Operative.CashRegister", "BeginDate", c => c.DateTime(nullable: false));
            AddColumn("Operative.CashRegister", "IsClosed", c => c.Boolean(nullable: false));
            AlterColumn("Operative.CashRegister", "UserClose", c => c.String());
            AlterColumn("Operative.CashRegister", "UserOpen", c => c.String());
            DropColumn("Operative.CashRegister", "ClosingDate");
            DropColumn("Operative.CashRegister", "OpeningDate");
        }
    }
}
