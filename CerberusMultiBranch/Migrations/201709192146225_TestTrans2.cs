namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestTrans2 : DbMigration
    {
        public override void Up()
        {

            AddColumn("Operative.Transaction", "TransactionFolio1", c => c.String(maxLength: 30));
            
            DropColumn("Operative.Transaction", "Folio1");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Transaction", "Folio1", c => c.String(maxLength: 30));
            DropColumn("Operative.Transaction", "TransactionFolio1");
            
        }
    }
}
