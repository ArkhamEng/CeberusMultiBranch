namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remDesFromProd : DbMigration
    {
        public override void Up()
        {
            DropColumn("Catalog.Product", "Description");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Product", "Description", c => c.String(maxLength: 200));
        }
    }
}
