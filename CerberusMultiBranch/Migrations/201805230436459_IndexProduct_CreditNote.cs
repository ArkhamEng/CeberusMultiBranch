namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndexProduct_CreditNote : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Product", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_LockDate");
            DropIndex("Catalog.Product", "IDX_UserLock");
            DropIndex("Operative.SaleCreditNote", "IDX_Year");
            DropIndex("Operative.SaleCreditNote", "IDX_Sequential");
            DropIndex("Operative.SaleCreditNote", "IDX_Ident");
            RenameIndex(table: "Catalog.Product", name: "IDX_CategoryId", newName: "IX_CategoryId");
            RenameIndex(table: "Catalog.Product", name: "IDX_PartSystemId", newName: "IX_PartSystemId");
            RenameIndex(table: "Catalog.Product", name: "IDX_Code", newName: "UNQ_Code");
            CreateIndex("Catalog.Product", new[] { "Code", "Name", "TradeMark" }, name: "IDX_Ident_TradeMark");
            CreateIndex("Catalog.Product", new[] { "LockDate", "UserLock" }, name: "IDX_Lock");
            CreateIndex("Operative.SaleCreditNote", new[] { "Folio", "Ident", "IsActive" }, name: "IDX_Identifier_Active");
            CreateIndex("Operative.SaleCreditNote", new[] { "Year", "Sequential" }, name: "IDX_Sequential");
        }
        
        public override void Down()
        {
            DropIndex("Operative.SaleCreditNote", "IDX_Sequential");
            DropIndex("Operative.SaleCreditNote", "IDX_Identifier_Active");
            DropIndex("Catalog.Product", "IDX_Lock");
            DropIndex("Catalog.Product", "IDX_Ident_TradeMark");
            RenameIndex(table: "Catalog.Product", name: "UNQ_Code", newName: "IDX_Code");
            RenameIndex(table: "Catalog.Product", name: "IX_PartSystemId", newName: "IDX_PartSystemId");
            RenameIndex(table: "Catalog.Product", name: "IX_CategoryId", newName: "IDX_CategoryId");
            CreateIndex("Operative.SaleCreditNote", "Ident", name: "IDX_Ident");
            CreateIndex("Operative.SaleCreditNote", "Sequential", name: "IDX_Sequential");
            CreateIndex("Operative.SaleCreditNote", "Year", name: "IDX_Year");
            CreateIndex("Catalog.Product", "UserLock", name: "IDX_UserLock");
            CreateIndex("Catalog.Product", "LockDate", name: "IDX_LockDate");
            CreateIndex("Catalog.Product", "Name", name: "IDX_Name");
        }
    }
}
