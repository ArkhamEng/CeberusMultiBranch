namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefundSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.SaleDetail", "Refund", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.SaleDetail", "Refund");
        }
    }
}
