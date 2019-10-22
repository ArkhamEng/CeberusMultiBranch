namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnlineStore : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.Branch", "ShowInMap", c => c.Boolean(nullable: false));
            AddColumn("Config.Branch", "IsWebStore", c => c.Boolean(nullable: false));
            AddColumn("Operative.BranchProduct", "OnlinePercentage", c => c.Int(nullable: false));
            AddColumn("Operative.BranchProduct", "OnlinePrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.BranchProduct", "OnlinePrice");
            DropColumn("Operative.BranchProduct", "OnlinePercentage");
            DropColumn("Config.Branch", "IsWebStore");
            DropColumn("Config.Branch", "ShowInMap");
        }
    }
}
