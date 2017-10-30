namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transference : DbMigration
    {
        public override void Up()
        {
            DropColumn("Operative.Transaction", "AuthUser");
            DropColumn("Operative.Transaction", "AuthDate");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Transaction", "AuthDate", c => c.DateTime());
            AddColumn("Operative.Transaction", "AuthUser", c => c.String());
        }
    }
}
