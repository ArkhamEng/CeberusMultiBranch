namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Purchase11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Operative.Purchase", "Bill", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("Operative.Purchase", "Bill", c => c.String(maxLength: 30));
        }
    }
}
