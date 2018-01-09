namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransToSale3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.Transaction", "Client_ClientId", "Catalog.Client");
            DropForeignKey("Operative.Transaction", "ProviderId", "Catalog.Provider");

            DropIndex("Operative.Transaction", new[] { "Client_ClientId" });


        }

        public override void Down()
        {
        }
    }
}
