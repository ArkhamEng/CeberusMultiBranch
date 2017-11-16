namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeEmail : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Employee", "IDX_Email");
        }
        
        public override void Down()
        {
            CreateIndex("Catalog.Employee", "Email", unique: true, name: "IDX_Email");
        }
    }
}
