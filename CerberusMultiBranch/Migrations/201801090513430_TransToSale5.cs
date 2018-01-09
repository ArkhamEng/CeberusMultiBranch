namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransToSale5 : DbMigration
    {
        public override void Up()
        {

            DropForeignKey("Operative.TransactionDetail", "FK_Operative.TransactionDetail_Operative.Transaction_TransactionId");
            DropForeignKey("Operative.Payment", "FK_Operative.Payment_Operative.Transaction_TransactionId");
            DropForeignKey("Operative.Sale", "FK_Operative.Transaction_Config.Branch_BranchId");
            DropForeignKey("Operative.Sale", "FK_Operative.Transaction_Catalog.Client_ClientId");
            DropForeignKey("Operative.Sale", "FK_Operative.Transaction_Security.AspNetUsers_UserId");

            DropPrimaryKey("Operative.Sale", "PK_Operative.Transaction");

            RenameColumn(table: "Operative.TransactionDetail", name: "TransactionId", newName: "SaleId");
            RenameTable(name: "Operative.TransactionDetail", newName: "SaleDetail");
            
            RenameIndex(table: "Operative.SaleDetail", name: "IX_TransactionId", newName: "IX_SaleId");

            RenameColumn(table: "Operative.Payment", name: "TransactionId", newName: "SaleId");
            RenameColumn(table: "Operative.Sale", name: "TransactionId", newName: "SaleId");
            AddPrimaryKey("Operative.Sale", "SaleId");

            AddForeignKey("Operative.SaleDetail", "SaleId", "Operative.Sale", "SaleId", cascadeDelete: true);
            AddForeignKey("Operative.Payment", "SaleId", "Operative.Sale", "SaleId");
            AddForeignKey("Operative.Sale", "BranchId", "Config.Branch", "BranchId", cascadeDelete: true);
            AddForeignKey("Operative.Sale", "ClientId", "Catalog.Client", "ClientId", cascadeDelete: true);
            AddForeignKey("Operative.Sale", "UserId", "Security.AspNetUsers", "Id", cascadeDelete: true);


            DropColumn("Operative.Sale", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Sale", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("Operative.Sale", "TransactionId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("Operative.SaleDetail", "SaleId", "Operative.Sale");
            DropForeignKey("Operative.Sale", "ClientId", "Catalog.Client");
            DropForeignKey("Operative.Payment", "Transaction_SaleId", "Operative.Sale");
            DropIndex("Operative.Payment", new[] { "Transaction_SaleId" });
            DropIndex("Operative.Sale", new[] { "ClientId" });
            DropPrimaryKey("Operative.Sale");
            AlterColumn("Operative.Sale", "ClientId", c => c.Int());
            DropColumn("Operative.Payment", "Transaction_SaleId");
            DropColumn("Operative.Sale", "SaleId");
            AddPrimaryKey("Operative.Sale", "TransactionId");
            RenameIndex(table: "Operative.SaleDetail", name: "IX_SaleId", newName: "IX_TransactionId");
            RenameColumn(table: "Operative.SaleDetail", name: "SaleId", newName: "TransactionId");
            RenameColumn(table: "Operative.Sale", name: "ClientId", newName: "Client_ClientId");
            AddColumn("Operative.Sale", "ClientId", c => c.Int(nullable: false));
            CreateIndex("Operative.Payment", "TransactionId");
            CreateIndex("Operative.Sale", "Client_ClientId");
            CreateIndex("Operative.Sale", "ClientId");
            AddForeignKey("Operative.Sale", "Client_ClientId", "Catalog.Client", "ClientId");
            AddForeignKey("Operative.Payment", "TransactionId", "Operative.Sale", "TransactionId", cascadeDelete: true);
            AddForeignKey("Operative.TransactionDetail", "TransactionId", "Operative.Sale", "TransactionId", cascadeDelete: true);
            RenameTable(name: "Operative.SaleDetail", newName: "TransactionDetail");
        }
    }
}
