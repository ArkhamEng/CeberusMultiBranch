namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SatCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.Category", "SatCode", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("Config.Category", "SatCode");
        }
    }
}
