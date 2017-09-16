namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChanges : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Catalog.Employee");
            AddColumn("Catalog.Employee", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Catalog.Employee", "EmployeeId", c => c.Int(nullable: false));
            AddPrimaryKey("Catalog.Employee", "UserId");
            CreateIndex("Catalog.Employee", "UserId");
            AddForeignKey("Catalog.Employee", "UserId", "Security.AspNetUsers", "Id");
            DropColumn("Security.AspNetUsers", "EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("Security.AspNetUsers", "EmployeeId", c => c.Int());
            DropForeignKey("Catalog.Employee", "UserId", "Security.AspNetUsers");
            DropIndex("Catalog.Employee", new[] { "UserId" });
            DropPrimaryKey("Catalog.Employee");
            AlterColumn("Catalog.Employee", "EmployeeId", c => c.Int(nullable: false, identity: true));
            DropColumn("Catalog.Employee", "UserId");
            AddPrimaryKey("Catalog.Employee", "EmployeeId");
        }
    }
}
