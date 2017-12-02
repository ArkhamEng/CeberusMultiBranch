namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchProductChanges1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("Operative.BranchProduct", "UpdUser");
        }
        
        public override void Down()
        {
            AddColumn("Operative.BranchProduct", "UpdUser", c => c.DateTime(nullable: false));
        }
    }
}
