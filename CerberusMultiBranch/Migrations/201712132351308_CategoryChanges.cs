namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("Config.Category", "IDX_Name");
            AddColumn("Config.Category", "UpdDate", c => c.DateTime(nullable: false));
            AddColumn("Config.Category", "UpdUser", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Config.Category", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Config.Category", "SatCode", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("Config.Category", "Name", unique: true, name: "IDX_Name");
            CreateIndex("Config.Category", "SatCode", unique: true, name: "IDX_SatCode");
        }
        
        public override void Down()
        {
            DropIndex("Config.Category", "IDX_SatCode");
            DropIndex("Config.Category", "IDX_Name");
            AlterColumn("Config.Category", "SatCode", c => c.String(maxLength: 30));
            AlterColumn("Config.Category", "Name", c => c.String(maxLength: 100));
            DropColumn("Config.Category", "UpdUser");
            DropColumn("Config.Category", "UpdDate");
            CreateIndex("Config.Category", "Name", name: "IDX_Name");
        }
    }
}
