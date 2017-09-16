namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addmigration : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Employee", "IDX_FTR");
            DropIndex("Catalog.Employee", "IDX_Email");
            CreateIndex("Catalog.Employee", "Name", unique: true, name: "IDX_Name");
            CreateIndex("Catalog.Employee", "FTR", unique: true, name: "IDX_FTR");
            CreateIndex("Catalog.Employee", "Email", unique: true, name: "IDX_Email");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Employee", "IDX_Email");
            DropIndex("Catalog.Employee", "IDX_FTR");
            DropIndex("Catalog.Employee", "IDX_Name");
            CreateIndex("Catalog.Employee", "Email", name: "IDX_Email");
            CreateIndex("Catalog.Employee", "FTR", name: "IDX_FTR");
        }
    }
}
