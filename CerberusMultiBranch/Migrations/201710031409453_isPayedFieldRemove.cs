namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isPayedFieldRemove : DbMigration
    {
        public override void Up()
        {
            DropColumn("Operative.Transaction", "IsPayed");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Transaction", "IsPayed", c => c.Boolean(nullable: false));
        }
    }
}
