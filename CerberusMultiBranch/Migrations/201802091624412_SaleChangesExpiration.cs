namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleChangesExpiration : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Sale", "Expiration", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.Sale", "Expiration");
        }
    }
}
