namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.BranchProduct", "MaxQuantity", c => c.Double(nullable: false));
            AddColumn("Operative.BranchProduct", "MinQuantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.BranchProduct", "MinQuantity");
            DropColumn("Operative.BranchProduct", "MaxQuantity");
        }
    }
}
