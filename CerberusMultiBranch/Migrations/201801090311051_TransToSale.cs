namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransToSale : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Operative.Transaction", newName: "Sale");
        }
        
        public override void Down()
        {
            RenameTable(name: "Operative.Sale", newName: "Transaction");
        }
    }
}
