namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branch_p1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.BranchProduct", "UpdUser", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("Operative.BranchProduct", "UpdUser");
        }
    }
}
