namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProvidereProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Provider", "CreditLimit", c => c.Double(nullable: false));
            AddColumn("Catalog.Provider", "DaysToPay", c => c.Int(nullable: false));
            AddColumn("Catalog.Provider", "Catalog", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Provider", "Catalog");
            DropColumn("Catalog.Provider", "DaysToPay");
            DropColumn("Catalog.Provider", "CreditLimit");
        }
    }
}
