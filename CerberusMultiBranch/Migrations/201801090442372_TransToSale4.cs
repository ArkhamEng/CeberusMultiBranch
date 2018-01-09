namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransToSale4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.Sale", "Client_ClientId", "Catalog.Client");
            DropForeignKey("Operative.Sale", "FK_Operative.Transaction_Catalog.Provider_ProviderId");

            DropIndex("Operative.Sale", new[] { "Client_ClientId" });


            DropColumn("Operative.Sale", "ProviderId");
            DropColumn("Operative.Sale", "Client_ClientId");
            DropColumn("Operative.Sale", "Bill");
            DropColumn("Operative.Sale", "Expiration");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Sale", "Expiration", c => c.DateTime());
            AddColumn("Operative.Sale", "Bill", c => c.String(maxLength: 30));
            AddColumn("Operative.Sale", "ProviderId", c => c.Int());
        }
    }
}
