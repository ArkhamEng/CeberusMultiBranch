namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChanges : DbMigration
    {
        public override void Up()
        {
            DropColumn("Catalog.Product", "BuyPrice");
            DropColumn("Catalog.Product", "StorePercentage");
            DropColumn("Catalog.Product", "DealerPercentage");
            DropColumn("Catalog.Product", "WholesalerPercentage");
            DropColumn("Catalog.Product", "StorePrice");
            DropColumn("Catalog.Product", "WholesalerPrice");
            DropColumn("Catalog.Product", "DealerPrice");
            DropColumn("Catalog.Product", "Row");
            DropColumn("Catalog.Product", "Ledge");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Product", "Ledge", c => c.String(maxLength: 30));
            AddColumn("Catalog.Product", "Row", c => c.String(maxLength: 30));
            AddColumn("Catalog.Product", "DealerPrice", c => c.Double(nullable: false));
            AddColumn("Catalog.Product", "WholesalerPrice", c => c.Double(nullable: false));
            AddColumn("Catalog.Product", "StorePrice", c => c.Double(nullable: false));
            AddColumn("Catalog.Product", "WholesalerPercentage", c => c.Int(nullable: false));
            AddColumn("Catalog.Product", "DealerPercentage", c => c.Int(nullable: false));
            AddColumn("Catalog.Product", "StorePercentage", c => c.Int(nullable: false));
            AddColumn("Catalog.Product", "BuyPrice", c => c.Double(nullable: false));
        }
    }
}
