namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockLocked : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.BranchProduct", "StockLocked", c => c.Boolean(nullable: false));
            DropColumn("Operative.BranchProduct", "IsLocked");
        }
        
        public override void Down()
        {
            AddColumn("Operative.BranchProduct", "IsLocked", c => c.Boolean(nullable: false));
            DropColumn("Operative.BranchProduct", "StockLocked");
        }
    }
}
