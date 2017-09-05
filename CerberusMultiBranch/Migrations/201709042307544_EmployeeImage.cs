namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeImage : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "Inventory.ProductImage", newSchema: "Catalog");
            AddColumn("Catalog.Employee", "PictureType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Employee", "PictureType");
            MoveTable(name: "Catalog.ProductImage", newSchema: "Inventory");
        }
    }
}
