namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsPropertiesTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Sale", "InsDate", c => c.DateTime(nullable: false));
            AddColumn("Operative.Sale", "InsUser", c => c.String(nullable: false));
            AddColumn("Operative.Purchase", "InsDate", c => c.DateTime(nullable: false));
            AddColumn("Operative.Purchase", "InsUser", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Operative.Purchase", "InsUser");
            DropColumn("Operative.Purchase", "InsDate");
            DropColumn("Operative.Sale", "InsUser");
            DropColumn("Operative.Sale", "InsDate");
        }
    }
}
