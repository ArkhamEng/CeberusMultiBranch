namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComissionOnSystem : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.PartSystem", "Commission", c => c.Int(nullable: false));
            DropColumn("Config.Category", "Commission");
        }
        
        public override void Down()
        {
            AddColumn("Config.Category", "Commission", c => c.Int(nullable: false));
            DropColumn("Config.PartSystem", "Commission");
        }
    }
}
