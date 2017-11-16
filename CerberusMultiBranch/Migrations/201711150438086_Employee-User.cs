namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeUser : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Employee", "IDX_UserId");
            CreateIndex("Catalog.Employee", "UserId", name: "IDX_UserId");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Employee", "IDX_UserId");
            CreateIndex("Catalog.Employee", "UserId", unique: true, name: "IDX_UserId");
        }
    }
}
