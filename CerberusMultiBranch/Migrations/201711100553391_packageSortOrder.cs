namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class packageSortOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.TransactionDetail", "SortOrder", c => c.Int(nullable: false));
            AddColumn("Operative.TransactionDetail", "ParentId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Operative.TransactionDetail", "ParentId");
            DropColumn("Operative.TransactionDetail", "SortOrder");
        }
    }
}
