namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComissionS : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Transaction", "ComPer", c => c.Int());
            AddColumn("Operative.Transaction", "ComAmount", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("Operative.Transaction", "ComAmount");
            DropColumn("Operative.Transaction", "ComPer");
        }
    }
}
