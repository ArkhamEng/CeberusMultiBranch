namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommissionPerCategory1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.TransactionDetail", "Commission", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.TransactionDetail", "Commission");
        }
    }
}
