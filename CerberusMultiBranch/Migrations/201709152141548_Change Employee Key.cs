namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEmployeeKey : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Employee", new[] { "UserId" });
            DropPrimaryKey("Catalog.Employee");
            AlterColumn("Catalog.Employee", "UserId", c => c.String(maxLength: 128));
            AlterColumn("Catalog.Employee", "EmployeeId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("Catalog.Employee", "EmployeeId");
            CreateIndex("Catalog.Employee", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Employee", new[] { "UserId" });
            DropPrimaryKey("Catalog.Employee");
            AlterColumn("Catalog.Employee", "EmployeeId", c => c.Int(nullable: false));
            AlterColumn("Catalog.Employee", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("Catalog.Employee", "UserId");
            CreateIndex("Catalog.Employee", "UserId");
        }
    }
}
