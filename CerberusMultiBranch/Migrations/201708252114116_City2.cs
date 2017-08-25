namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class City2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Common.City", "IDX_Name");
        }
        
        public override void Down()
        {
            CreateIndex("Common.City", "Name", unique: true, name: "IDX_Name");
        }
    }
}
