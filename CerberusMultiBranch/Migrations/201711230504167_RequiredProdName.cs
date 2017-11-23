namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredProdName : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Product", "IDX_Name");
            AlterColumn("Catalog.Product", "Name", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("Catalog.Product", "Name", name: "IDX_Name");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Product", "IDX_Name");
            AlterColumn("Catalog.Product", "Name", c => c.String(maxLength: 200));
            CreateIndex("Catalog.Product", "Name", name: "IDX_Name");
        }
    }
}
