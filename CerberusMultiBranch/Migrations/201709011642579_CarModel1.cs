namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarModel1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("Config.CarModel", "CarYearId");
        }
        
        public override void Down()
        {
            AddColumn("Config.CarModel", "CarYearId", c => c.Int(nullable: false));
        }
    }
}
