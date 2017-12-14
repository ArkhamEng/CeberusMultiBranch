namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartSystemChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.PartSystem", "UpdDate", c => c.DateTime(nullable: false));
            AddColumn("Config.PartSystem", "UpdUser", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Config.PartSystem", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("Config.PartSystem", "Name", unique: true, name: "IDX_Name");
        }
        
        public override void Down()
        {
            DropIndex("Config.PartSystem", "IDX_Name");
            AlterColumn("Config.PartSystem", "Name", c => c.String());
            DropColumn("Config.PartSystem", "UpdUser");
            DropColumn("Config.PartSystem", "UpdDate");
        }
    }
}
