namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseDetail1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Operative.PurchaseDetail", "ProductId");
            AddForeignKey("Operative.PurchaseDetail", "ProductId", "Catalog.Product", "ProductId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.PurchaseDetail", "ProductId", "Catalog.Product");
            DropIndex("Operative.PurchaseDetail", new[] { "ProductId" });
        }
    }
}
