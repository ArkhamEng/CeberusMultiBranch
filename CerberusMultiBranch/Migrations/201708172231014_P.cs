namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class P : DbMigration
    {
        public override void Up()
        {
            AddColumn("Inventory.ProductFile", "Size", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Inventory.ProductFile", "Size");
        }
    }
}
