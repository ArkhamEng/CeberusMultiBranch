namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Client", "CreditLimit", c => c.Double(nullable: false));
            AddColumn("Catalog.Client", "UsedAmount", c => c.Double(nullable: false));
            DropColumn("Catalog.Client", "LegalRepresentative");
            DropColumn("Catalog.Client", "TaxAddress");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Client", "TaxAddress", c => c.String());
            AddColumn("Catalog.Client", "LegalRepresentative", c => c.String(maxLength: 100));
            DropColumn("Catalog.Client", "UsedAmount");
            DropColumn("Catalog.Client", "CreditLimit");
        }
    }
}
