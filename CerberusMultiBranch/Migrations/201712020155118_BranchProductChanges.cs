namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchProductChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.BranchProduct", "UpdUser", c => c.DateTime(nullable: false));
            AddColumn("Operative.BranchProduct", "BuyPrice", c => c.Double(nullable: false));
            AddColumn("Operative.BranchProduct", "StorePercentage", c => c.Int(nullable: false));
            AddColumn("Operative.BranchProduct", "DealerPercentage", c => c.Int(nullable: false));
            AddColumn("Operative.BranchProduct", "WholesalerPercentage", c => c.Int(nullable: false));
            AddColumn("Operative.BranchProduct", "StorePrice", c => c.Double(nullable: false));
            AddColumn("Operative.BranchProduct", "WholesalerPrice", c => c.Double(nullable: false));
            AddColumn("Operative.BranchProduct", "DealerPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.BranchProduct", "DealerPrice");
            DropColumn("Operative.BranchProduct", "WholesalerPrice");
            DropColumn("Operative.BranchProduct", "StorePrice");
            DropColumn("Operative.BranchProduct", "WholesalerPercentage");
            DropColumn("Operative.BranchProduct", "DealerPercentage");
            DropColumn("Operative.BranchProduct", "StorePercentage");
            DropColumn("Operative.BranchProduct", "BuyPrice");
            DropColumn("Operative.BranchProduct", "UpdUser");
        }
    }
}
