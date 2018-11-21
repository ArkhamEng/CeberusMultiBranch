namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredMakeModelFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Config.CarModel", "Name", c => c.String(nullable: false));
            AlterColumn("Config.CarMake", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Config.CarMake", "Name", c => c.String());
            AlterColumn("Config.CarModel", "Name", c => c.String());
        }
    }
}
