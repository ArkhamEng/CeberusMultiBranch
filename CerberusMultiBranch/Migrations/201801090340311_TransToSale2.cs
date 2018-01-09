namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransToSale2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.Sale", "ProviderId", "Catalog.Provider");
            DropForeignKey("Operative.TransactionDetail", "TransactionId", "Operative.Sale");
            DropIndex("Operative.Sale", new[] { "ProviderId" });
        }
        
        public override void Down()
        {
            CreateIndex("Operative.Sale", "ProviderId");
            AddForeignKey("Operative.TransactionDetail", "TransactionId", "Operative.Sale", "TransactionId", cascadeDelete: true);
            AddForeignKey("Operative.Sale", "ProviderId", "Catalog.Provider", "ProviderId", cascadeDelete: true);
        }
    }
}
