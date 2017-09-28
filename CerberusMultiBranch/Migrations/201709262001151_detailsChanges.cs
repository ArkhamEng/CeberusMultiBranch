namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class detailsChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.Transaction", "ClientId", "Catalog.Client");
            DropPrimaryKey("Operative.TransactionDetail");
            AddPrimaryKey("Operative.TransactionDetail", new[] { "TransactionId", "ProductId" });
            AddForeignKey("Operative.Transaction", "ClientId", "Catalog.Client", "ClientId");
            DropColumn("Operative.TransactionDetail", "TransactionDetailId");
        }
        
        public override void Down()
        {
            AddColumn("Operative.TransactionDetail", "TransactionDetailId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("Operative.Transaction", "ClientId", "Catalog.Client");
            DropPrimaryKey("Operative.TransactionDetail");
            AddPrimaryKey("Operative.TransactionDetail", "TransactionDetailId");
            AddForeignKey("Operative.Transaction", "ClientId", "Catalog.Client", "ClientId", cascadeDelete: true);
        }
    }
}
