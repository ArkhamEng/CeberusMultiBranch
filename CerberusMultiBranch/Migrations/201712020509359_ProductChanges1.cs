namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChanges1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.BranchProduct", "Row", c => c.String(maxLength: 30));
            AddColumn("Operative.BranchProduct", "Ledge", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("Operative.BranchProduct", "Ledge");
            DropColumn("Operative.BranchProduct", "Row");
        }
    }
}
