namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompositeKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Config.EmployeeBranch");
            AddPrimaryKey("Config.EmployeeBranch", new[] { "BranchId", "EmployeeId" });
            DropColumn("Config.EmployeeBranch", "EmployeeBranchId");
        }
        
        public override void Down()
        {
            AddColumn("Config.EmployeeBranch", "EmployeeBranchId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("Config.EmployeeBranch");
            AddPrimaryKey("Config.EmployeeBranch", "EmployeeBranchId");
        }
    }
}
