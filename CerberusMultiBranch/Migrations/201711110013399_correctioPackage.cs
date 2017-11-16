namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctioPackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Product", "MaxQuantity", c => c.Double(nullable: false));
            DropColumn("Catalog.Product", "PackagePrice");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Product", "PackagePrice", c => c.Double(nullable: false));
            DropColumn("Catalog.Product", "MaxQuantity");
        }
    }
}
