namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeFtr : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Employee", "IDX_FTR");
        }
        
        public override void Down()
        {
            CreateIndex("Catalog.Employee", "FTR", unique: true, name: "IDX_FTR");
        }
    }
}
