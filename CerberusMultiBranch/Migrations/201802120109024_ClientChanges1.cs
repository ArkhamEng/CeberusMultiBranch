namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientChanges1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Client", "CreditDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Client", "CreditDays");
        }
    }
}
