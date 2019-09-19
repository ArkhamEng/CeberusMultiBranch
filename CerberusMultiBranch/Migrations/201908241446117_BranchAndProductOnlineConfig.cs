namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchAndProductOnlineConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.Branch", "Latitude", c => c.Double(nullable: false));
            AddColumn("Config.Branch", "Longitude", c => c.Double(nullable: false));
            AddColumn("Config.Branch", "Image", c => c.String());
            AddColumn("Config.Branch", "Phone", c => c.String());
            AddColumn("Catalog.Product", "IsOnlineSold", c => c.Boolean(nullable: false));
            AddColumn("Catalog.Product", "OnlinePercentage", c => c.Int(nullable: false));
            AddColumn("Catalog.Product", "OnlinePrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Product", "OnlinePrice");
            DropColumn("Catalog.Product", "OnlinePercentage");
            DropColumn("Catalog.Product", "IsOnlineSold");
            DropColumn("Config.Branch", "Phone");
            DropColumn("Config.Branch", "Image");
            DropColumn("Config.Branch", "Longitude");
            DropColumn("Config.Branch", "Latitude");
        }
    }
}
