namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Purchase1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Operative.Purchase", "PurchaseDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Operative.Purchase", "PurchaseDate", c => c.String(nullable: false));
        }
    }
}
