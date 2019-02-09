namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPurchaseItems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Purchasing.PurchaseItem", "PurchaseTypeId", "Purchasing.PurchaseType");
            DropIndex("Purchasing.PurchaseItem", new[] { "PurchaseTypeId" });
            RenameColumn(table: "Purchasing.PurchaseItem", name: "PurchaseTypeId", newName: "PurchaseType_PurchaseTypeId");
            AlterColumn("Purchasing.PurchaseItem", "PurchaseType_PurchaseTypeId", c => c.Int());
            CreateIndex("Purchasing.PurchaseItem", "PurchaseType_PurchaseTypeId");
            AddForeignKey("Purchasing.PurchaseItem", "PurchaseType_PurchaseTypeId", "Purchasing.PurchaseType", "PurchaseTypeId");
            DropColumn("Purchasing.PurchaseItem", "Comment");
            DropColumn("Purchasing.PurchaseItem", "DaysToPay");
            DropColumn("Purchasing.PurchaseItem", "TaxRate");
        }
        
        public override void Down()
        {
            AddColumn("Purchasing.PurchaseItem", "TaxRate", c => c.Double(nullable: false));
            AddColumn("Purchasing.PurchaseItem", "DaysToPay", c => c.Int(nullable: false));
            AddColumn("Purchasing.PurchaseItem", "Comment", c => c.String());
            DropForeignKey("Purchasing.PurchaseItem", "PurchaseType_PurchaseTypeId", "Purchasing.PurchaseType");
            DropIndex("Purchasing.PurchaseItem", new[] { "PurchaseType_PurchaseTypeId" });
            AlterColumn("Purchasing.PurchaseItem", "PurchaseType_PurchaseTypeId", c => c.Int(nullable: false));
            RenameColumn(table: "Purchasing.PurchaseItem", name: "PurchaseType_PurchaseTypeId", newName: "PurchaseTypeId");
            CreateIndex("Purchasing.PurchaseItem", "PurchaseTypeId");
            AddForeignKey("Purchasing.PurchaseItem", "PurchaseTypeId", "Purchasing.PurchaseType", "PurchaseTypeId", cascadeDelete: true);
        }
    }
}
