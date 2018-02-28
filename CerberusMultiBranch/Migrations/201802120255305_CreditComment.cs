namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Client", "CreditComment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Client", "CreditComment");
        }
    }
}
