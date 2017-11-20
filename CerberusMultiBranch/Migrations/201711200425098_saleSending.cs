namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saleSending : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Transaction", "SendingType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Operative.Transaction", "SendingType");
        }
    }
}
