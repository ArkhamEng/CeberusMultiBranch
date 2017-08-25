namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cities : DbMigration
    {
        public override void Up()
        {
            DropIndex("Common.City", "IDX_Name");
            AlterColumn("Common.City", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("Common.City", "Name", unique: true, name: "IDX_Name");
        }
        
        public override void Down()
        {
            DropIndex("Common.City", "IDX_Name");
            AlterColumn("Common.City", "Name", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("Common.City", "Name", unique: true, name: "IDX_Name");
        }
    }
}
