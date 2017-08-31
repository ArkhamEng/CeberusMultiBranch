namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Year : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Config.Make", newName: "CarMake");
        }
        
        public override void Down()
        {
            RenameTable(name: "Config.CarMake", newName: "Make");
        }
    }
}
