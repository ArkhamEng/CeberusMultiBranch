namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImagesChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.ProductImage", "Path", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("Catalog.ProductImage", "Name", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("Catalog.ProductImage", "Type", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("Catalog.ProductImage", "Size", c => c.Int(nullable: false));
            DropColumn("Catalog.ProductImage", "File");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.ProductImage", "File", c => c.Binary());
            AlterColumn("Catalog.ProductImage", "Size", c => c.Double(nullable: false));
            AlterColumn("Catalog.ProductImage", "Type", c => c.String());
            AlterColumn("Catalog.ProductImage", "Name", c => c.String());
            DropColumn("Catalog.ProductImage", "Path");
        }
    }
}
