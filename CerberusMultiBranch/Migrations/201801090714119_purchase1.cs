namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchase1 : DbMigration
    {
        public override void Up()
        {
            return;
            /*
            DropForeignKey("Operative.Payment", "Transaction_SaleId", "Operative.Sale");
            DropIndex("Operative.Payment", new[] { "Transaction_SaleId" });
            RenameColumn(table: "Operative.Payment", name: "Transaction_SaleId", newName: "SaleId");
            AlterColumn("Operative.Payment", "SaleId", c => c.Int(nullable: false));
            CreateIndex("Operative.Payment", "SaleId");
            AddForeignKey("Operative.Payment", "SaleId", "Operative.Sale", "SaleId", cascadeDelete: true);
            DropColumn("Operative.Payment", "TransactionId");*/
        }
        
        public override void Down()
        {
            AddColumn("Operative.Payment", "TransactionId", c => c.Int(nullable: false));
            DropForeignKey("Operative.Payment", "SaleId", "Operative.Sale");
            DropIndex("Operative.Payment", new[] { "SaleId" });
            AlterColumn("Operative.Payment", "SaleId", c => c.Int());
            RenameColumn(table: "Operative.Payment", name: "SaleId", newName: "Transaction_SaleId");
            CreateIndex("Operative.Payment", "Transaction_SaleId");
            AddForeignKey("Operative.Payment", "Transaction_SaleId", "Operative.Sale", "SaleId");
        }
    }
}
