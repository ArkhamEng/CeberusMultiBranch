namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleChangesRemoveMethod : DbMigration
    {
        public override void Up()
        {
            DropColumn("Operative.Sale", "PaymentMethod");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Sale", "PaymentMethod", c => c.Int(nullable: false));
        }
    }
}
