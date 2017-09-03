namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductNameLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Product", "IDX_Name");
            AlterColumn("Catalog.Product", "Name", c => c.String(maxLength: 100));
            CreateIndex("Catalog.Product", "Name", unique: true, name: "IDX_Name");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Product", "IDX_Name");
            AlterColumn("Catalog.Product", "Name", c => c.String(maxLength: 20));
            CreateIndex("Catalog.Product", "Name", unique: true, name: "IDX_Name");
        }
    }
}
