namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PathLengthImage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Catalog.ProductImage", "Path", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("Catalog.ProductImage", "Path", c => c.String(nullable: false, maxLength: 150));
        }
    }
}
