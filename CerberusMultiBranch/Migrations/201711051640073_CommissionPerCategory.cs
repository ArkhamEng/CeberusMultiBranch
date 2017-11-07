namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommissionPerCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.Category", "Commission", c => c.Int(nullable: false));
            AddColumn("Catalog.Client", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Client", "Type");
            DropColumn("Config.Category", "Commission");
        }
    }
}
