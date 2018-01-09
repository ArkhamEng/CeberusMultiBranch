namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransToSale1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.Sale", "ClientId", "Catalog.Client");
            DropIndex("Operative.Sale", new[] { "ClientId" });
            AddColumn("Operative.Sale", "Client_ClientId", c => c.Int());
            AlterColumn("Operative.Sale", "ClientId", c => c.Int(nullable: false));
            AlterColumn("Operative.Sale", "Folio", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("Operative.Sale", "ComPer", c => c.Int(nullable: false));
            AlterColumn("Operative.Sale", "ComAmount", c => c.Double(nullable: false));
            AlterColumn("Operative.Sale", "SendingType", c => c.Int(nullable: false));
            CreateIndex("Operative.Sale", "ClientId");
            CreateIndex("Operative.Sale", "Client_ClientId");
            AddForeignKey("Operative.Sale", "Client_ClientId", "Catalog.Client", "ClientId");
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.Sale", "Client_ClientId", "Catalog.Client");
            DropIndex("Operative.Sale", new[] { "Client_ClientId" });
            DropIndex("Operative.Sale", new[] { "ClientId" });
            AlterColumn("Operative.Sale", "SendingType", c => c.Int());
            AlterColumn("Operative.Sale", "ComAmount", c => c.Double());
            AlterColumn("Operative.Sale", "ComPer", c => c.Int());
            AlterColumn("Operative.Sale", "Folio", c => c.String(maxLength: 30));
            AlterColumn("Operative.Sale", "ClientId", c => c.Int());
            DropColumn("Operative.Sale", "Client_ClientId");
            CreateIndex("Operative.Sale", "ClientId");
            AddForeignKey("Operative.Sale", "ClientId", "Catalog.Client", "ClientId", cascadeDelete: true);
        }
    }
}
