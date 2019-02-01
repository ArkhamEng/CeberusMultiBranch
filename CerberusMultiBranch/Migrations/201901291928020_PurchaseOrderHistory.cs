namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseOrderHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Purchasing.PurchaseOrderHistory",
                c => new
                    {
                        PurchaseOrderHistoryId = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Status = c.String(maxLength: 50),
                        Type = c.String(maxLength: 50),
                        ShipMethod = c.String(maxLength: 50),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ModifyByUser = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderHistoryId)
                .ForeignKey("Purchasing.PurchaseOrder", t => t.PurchaseOrderId, cascadeDelete: true)
                .Index(t => t.PurchaseOrderId);
            
            AddColumn("Purchasing.PurchaseOrder", "Comment", c => c.String());
            AddColumn("Purchasing.PurchaseOrder", "Year", c => c.Int(nullable: false));
            AddColumn("Purchasing.PurchaseOrder", "Sequential", c => c.Int(nullable: false));
            AlterColumn("Purchasing.PurchaseOrder", "OrderDate", c => c.DateTime());
            AlterColumn("Purchasing.PurchaseOrder", "DueDate", c => c.DateTime());
            AlterColumn("Purchasing.PurchaseOrder", "ShipDate", c => c.DateTime());
            AlterColumn("Purchasing.PurchaseOrder", "DeliveryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("Purchasing.PurchaseOrderHistory", "PurchaseOrderId", "Purchasing.PurchaseOrder");
            DropIndex("Purchasing.PurchaseOrderHistory", new[] { "PurchaseOrderId" });
            AlterColumn("Purchasing.PurchaseOrder", "DeliveryDate", c => c.DateTime(nullable: false));
            AlterColumn("Purchasing.PurchaseOrder", "ShipDate", c => c.DateTime(nullable: false));
            AlterColumn("Purchasing.PurchaseOrder", "DueDate", c => c.DateTime(nullable: false));
            AlterColumn("Purchasing.PurchaseOrder", "OrderDate", c => c.DateTime(nullable: false));
            DropColumn("Purchasing.PurchaseOrder", "Sequential");
            DropColumn("Purchasing.PurchaseOrder", "Year");
            DropColumn("Purchasing.PurchaseOrder", "Comment");
            DropTable("Purchasing.PurchaseOrderHistory");
        }
    }
}
