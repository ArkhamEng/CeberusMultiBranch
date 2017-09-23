namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Indexes : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Employee", new[] { "UserId" });
            DropIndex("Catalog.Product", "IDX_Name");
            DropIndex("Catalog.Provider", "IDX_BussinessName");
            DropIndex("Catalog.Provider", "IDX_FTR");
            RenameIndex(table: "Catalog.Employee", name: "IX_CityId", newName: "IDX_CityId");
            RenameIndex(table: "Common.City", name: "IX_StateId", newName: "IDX_StateId");
            RenameIndex(table: "Operative.Transaction", name: "IX_TransactionTypeId", newName: "IDX_TransactionTypeId");
            RenameIndex(table: "Operative.Transaction", name: "IX_BranchId", newName: "IDX_BranchId");
            RenameIndex(table: "Operative.TransactionDetail", name: "IX_TransactionId", newName: "IDX_TransactionId");
            RenameIndex(table: "Operative.TransactionDetail", name: "IX_ProductId", newName: "IDX_ProductId");
            RenameIndex(table: "Catalog.Product", name: "IX_CategoryId", newName: "IDX_CategoryId");
            RenameIndex(table: "Config.CarYear", name: "IX_CarModelId", newName: "IDX_CarModelId");
            RenameIndex(table: "Config.CarModel", name: "IX_CarMakeId", newName: "IDX_CarMakeId");
            RenameIndex(table: "Catalog.ProductImage", name: "IX_ProductId", newName: "IDX_ProductId");
            RenameIndex(table: "Catalog.Client", name: "IX_CityId", newName: "IDX_CityId");
            DropPrimaryKey("Catalog.Compatibility");
            AlterColumn("Catalog.Provider", "Name", c => c.String(nullable: false, maxLength: 100));
            AddPrimaryKey("Catalog.Compatibility", new[] { "CarYearId", "ProductId" });
            CreateIndex("Catalog.Employee", "UserId", unique: true, name: "IDX_UserId");
            CreateIndex("Operative.Transaction", "TransactionDate", name: "IDX_TransactionDate");
            CreateIndex("Operative.Transaction", "IsCompleated", name: "IDX_IsCompleated");
            CreateIndex("Catalog.Product", "Name", name: "IDX_Name");
            CreateIndex("Catalog.Provider", "Name", name: "IDX_Name");
            DropColumn("Catalog.Compatibility", "CompatibilityId");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Compatibility", "CompatibilityId", c => c.Int(nullable: false, identity: true));
            DropIndex("Catalog.Provider", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_Name");
            DropIndex("Operative.Transaction", "IDX_IsCompleated");
            DropIndex("Operative.Transaction", "IDX_TransactionDate");
            DropIndex("Catalog.Employee", "IDX_UserId");
            DropPrimaryKey("Catalog.Compatibility");
            AlterColumn("Catalog.Provider", "Name", c => c.String(nullable: false));
            AddPrimaryKey("Catalog.Compatibility", "CompatibilityId");
            RenameIndex(table: "Catalog.Client", name: "IDX_CityId", newName: "IX_CityId");
            RenameIndex(table: "Catalog.ProductImage", name: "IDX_ProductId", newName: "IX_ProductId");
            RenameIndex(table: "Config.CarModel", name: "IDX_CarMakeId", newName: "IX_CarMakeId");
            RenameIndex(table: "Config.CarYear", name: "IDX_CarModelId", newName: "IX_CarModelId");
            RenameIndex(table: "Catalog.Product", name: "IDX_CategoryId", newName: "IX_CategoryId");
            RenameIndex(table: "Operative.TransactionDetail", name: "IDX_ProductId", newName: "IX_ProductId");
            RenameIndex(table: "Operative.TransactionDetail", name: "IDX_TransactionId", newName: "IX_TransactionId");
            RenameIndex(table: "Operative.Transaction", name: "IDX_BranchId", newName: "IX_BranchId");
            RenameIndex(table: "Operative.Transaction", name: "IDX_TransactionTypeId", newName: "IX_TransactionTypeId");
            RenameIndex(table: "Common.City", name: "IDX_StateId", newName: "IX_StateId");
            RenameIndex(table: "Catalog.Employee", name: "IDX_CityId", newName: "IX_CityId");
            CreateIndex("Catalog.Provider", "FTR", name: "IDX_FTR");
            CreateIndex("Catalog.Provider", "BusinessName", name: "IDX_BussinessName");
            CreateIndex("Catalog.Product", "Name", unique: true, name: "IDX_Name");
            CreateIndex("Catalog.Employee", "UserId");
        }
    }
}
