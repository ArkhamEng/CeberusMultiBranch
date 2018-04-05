namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProdChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.BranchProduct", "IsLocked", c => c.Boolean(nullable: false));
            AddColumn("Catalog.Product", "StockRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Product", "StockRequired");
            DropColumn("Operative.BranchProduct", "IsLocked");
        }
    }
}
