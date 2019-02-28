namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Purchase_PurchaseOrder : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "Operative.Purchase", newSchema: "Purchasing");
            MoveTable(name: "Operative.PurchaseDetail", newSchema: "Purchasing");
            AddColumn("Purchasing.Purchase", "PurchaseOrderId", c => c.Int());
            CreateIndex("Purchasing.Purchase", "PurchaseOrderId");
            AddForeignKey("Purchasing.Purchase", "PurchaseOrderId", "Purchasing.PurchaseOrder", "PurchaseOrderId");
        }
        
        public override void Down()
        {
            DropForeignKey("Purchasing.Purchase", "PurchaseOrderId", "Purchasing.PurchaseOrder");
            DropIndex("Purchasing.Purchase", new[] { "PurchaseOrderId" });
            DropColumn("Purchasing.Purchase", "PurchaseOrderId");
            MoveTable(name: "Purchasing.PurchaseDetail", newSchema: "Operative");
            MoveTable(name: "Purchasing.Purchase", newSchema: "Operative");
        }
    }
}
