namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reservedProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.BranchProduct", "Reserved", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.BranchProduct", "Reserved");
        }
    }
}
