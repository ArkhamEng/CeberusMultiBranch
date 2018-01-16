namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Idontknow : DbMigration
    {
        public override void Up()
        {
            AddColumn("Finances.SalePayment", "UpdUser", c => c.String(maxLength: 100));
            AddColumn("Finances.SalePayment", "Comment", c => c.String(maxLength: 100));
            AddColumn("Finances.SalePayment", "Reference", c => c.String(maxLength: 30));
            AddColumn("Finances.SalePayment", "UpdDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Finances.SalePayment", "UpdDate");
            DropColumn("Finances.SalePayment", "Reference");
            DropColumn("Finances.SalePayment", "Comment");
            DropColumn("Finances.SalePayment", "UpdUser");
        }
    }
}
