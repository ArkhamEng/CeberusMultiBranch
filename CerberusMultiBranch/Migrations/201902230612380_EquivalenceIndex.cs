namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EquivalenceIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Equivalence", "IDX_ProviderId");
            DropIndex("Catalog.Equivalence", new[] { "ProductId" });
            CreateIndex("Catalog.Equivalence", new[] { "ProviderId", "ProductId" }, name: "IDX_Provider_Product");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Equivalence", "IDX_Provider_Product");
            CreateIndex("Catalog.Equivalence", "ProductId");
            CreateIndex("Catalog.Equivalence", "ProviderId", name: "IDX_ProviderId");
        }
    }
}
