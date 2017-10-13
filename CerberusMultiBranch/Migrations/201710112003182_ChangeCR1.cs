namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCR1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.CashRegister", "IsOpen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.CashRegister", "IsOpen");
        }
    }
}
