namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeBranches : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Config.EmployeeBranch",
                c => new
                    {
                        EmployeeBranchId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeBranchId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Employee", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Config.EmployeeBranch", "EmployeeId", "Catalog.Employee");
            DropForeignKey("Config.EmployeeBranch", "BranchId", "Config.Branch");
            DropIndex("Config.EmployeeBranch", new[] { "EmployeeId" });
            DropIndex("Config.EmployeeBranch", new[] { "BranchId" });
            DropTable("Config.EmployeeBranch");
        }
    }
}
