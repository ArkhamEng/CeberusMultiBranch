namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isPayedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Transaction", "IsPayed", c => c.Boolean(nullable: false));
            AddColumn("Operative.Transaction", "PaymentType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Operative.Transaction", "PaymentType");
            DropColumn("Operative.Transaction", "IsPayed");
        }
    }
}
