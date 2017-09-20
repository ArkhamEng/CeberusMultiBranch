namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestTransacntion : DbMigration
    {
        public override void Up()
        {
            DropIndex("Operative.Transaction", new[] { "ProviderId" });
            AddColumn("Operative.Transaction", "ClientId", c => c.Int());
            AddColumn("Operative.Transaction", "Folio", c => c.String(maxLength: 30));
            AddColumn("Operative.Transaction", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Operative.Transaction", "ProviderId", c => c.Int());
            CreateIndex("Operative.Transaction", "ProviderId");
            CreateIndex("Operative.Transaction", "ClientId");
            AddForeignKey("Operative.Transaction", "ClientId", "Catalog.Client", "ClientId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.Transaction", "ClientId", "Catalog.Client");
            DropIndex("Operative.Transaction", new[] { "ClientId" });
            DropIndex("Operative.Transaction", new[] { "ProviderId" });
            AlterColumn("Operative.Transaction", "ProviderId", c => c.Int(nullable: false));
            DropColumn("Operative.Transaction", "Discriminator");
            DropColumn("Operative.Transaction", "Folio");
            DropColumn("Operative.Transaction", "ClientId");
            CreateIndex("Operative.Transaction", "ProviderId");
        }
    }
}
