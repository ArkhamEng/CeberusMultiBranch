namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeFieldsFromCatalog : DbMigration
    {
        public override void Up()
        {
            DropColumn("Catalog.Client", "CityId");
            DropColumn("Catalog.Client", "Address");
            DropColumn("Catalog.Client", "ZipCode");
            DropColumn("Catalog.Employee", "CityId");
            DropColumn("Catalog.Employee", "Address");
            DropColumn("Catalog.Employee", "ZipCode");
            DropColumn("Catalog.Provider", "CityId");
            DropColumn("Catalog.Provider", "Address");
            DropColumn("Catalog.Provider", "ZipCode");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Provider", "ZipCode", c => c.String(maxLength: 10));
            AddColumn("Catalog.Provider", "Address", c => c.String(maxLength: 150));
            AddColumn("Catalog.Provider", "CityId", c => c.Int());
            AddColumn("Catalog.Employee", "ZipCode", c => c.String(maxLength: 6));
            AddColumn("Catalog.Employee", "Address", c => c.String(maxLength: 100));
            AddColumn("Catalog.Employee", "CityId", c => c.Int());
            AddColumn("Catalog.Client", "ZipCode", c => c.String(maxLength: 10));
            AddColumn("Catalog.Client", "Address", c => c.String(maxLength: 150));
            AddColumn("Catalog.Client", "CityId", c => c.Int());
        }
    }
}
