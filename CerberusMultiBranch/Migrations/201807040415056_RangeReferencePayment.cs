namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RangeReferencePayment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Finances.SalePayment", "Reference", c => c.String(maxLength: 100));
            AlterColumn("Finances.PurchasePayment", "Reference", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("Finances.PurchasePayment", "Reference", c => c.String(maxLength: 30));
            AlterColumn("Finances.SalePayment", "Reference", c => c.String(maxLength: 30));
        }
    }
}
