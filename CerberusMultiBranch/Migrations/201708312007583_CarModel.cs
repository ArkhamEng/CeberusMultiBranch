namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Config.CarModel", "Make_CarMakeId", "Config.CarMake");
            DropIndex("Config.CarModel", new[] { "Make_CarMakeId" });
            RenameColumn(table: "Config.CarModel", name: "Make_CarMakeId", newName: "CarMakeId");
            AlterColumn("Config.CarModel", "CarMakeId", c => c.Int(nullable: false));
            CreateIndex("Config.CarModel", "CarMakeId");
            AddForeignKey("Config.CarModel", "CarMakeId", "Config.CarMake", "CarMakeId", cascadeDelete: true);
            DropColumn("Config.CarModel", "MakeId");
        }
        
        public override void Down()
        {
            AddColumn("Config.CarModel", "MakeId", c => c.Int(nullable: false));
            DropForeignKey("Config.CarModel", "CarMakeId", "Config.CarMake");
            DropIndex("Config.CarModel", new[] { "CarMakeId" });
            AlterColumn("Config.CarModel", "CarMakeId", c => c.Int());
            RenameColumn(table: "Config.CarModel", name: "CarMakeId", newName: "Make_CarMakeId");
            CreateIndex("Config.CarModel", "Make_CarMakeId");
            AddForeignKey("Config.CarModel", "Make_CarMakeId", "Config.CarMake", "CarMakeId");
        }
    }
}
