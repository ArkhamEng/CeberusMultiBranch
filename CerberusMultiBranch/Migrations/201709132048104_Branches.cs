namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Branches : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Config.Branch",
                c => new
                    {
                        BranchId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.BranchId);
            
            CreateTable(
                "Operative.ProductInventory",
                c => new
                    {
                        ProductInventoryId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Stock = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProductInventoryId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.BranchId);

        }
        
        public override void Down()
        {
            DropForeignKey("Catalog.Provider", "CityId", "Common.City");
            DropForeignKey("Catalog.Employee", "CityId", "Common.City");
            DropForeignKey("Catalog.Client", "CityId", "Common.City");
            DropForeignKey("Common.City", "StateId", "Common.State");
            DropForeignKey("Operative.ProductInventory", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.ProductImage", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Compatibility", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Compatibility", "CarYearId", "Config.CarYear");
            DropForeignKey("Config.CarYear", "CarModelId", "Config.CarModel");
            DropForeignKey("Config.CarModel", "CarMakeId", "Config.CarMake");
            DropForeignKey("Catalog.Product", "CategoryId", "Config.Category");
            DropForeignKey("Operative.ProductInventory", "BranchId", "Config.Branch");
            DropIndex("Catalog.Provider", new[] { "CityId" });
            DropIndex("Catalog.Provider", "IDX_Phone");
            DropIndex("Catalog.Provider", "IDX_Email");
            DropIndex("Catalog.Provider", "IDX_FTR");
            DropIndex("Catalog.Provider", "IDX_BussinessName");
            DropIndex("Catalog.Provider", "IDX_Code");
            DropIndex("Catalog.Employee", new[] { "CityId" });
            DropIndex("Catalog.Employee", "IDX_Phone");
            DropIndex("Catalog.Employee", "IDX_Email");
            DropIndex("Catalog.Employee", "IDX_FTR");
            DropIndex("Catalog.Employee", "IDX_Code");
            DropIndex("Catalog.Client", new[] { "CityId" });
            DropIndex("Catalog.Client", "IDX_Phone");
            DropIndex("Catalog.Client", "IDX_Email");
            DropIndex("Catalog.Client", "IDX_FTR");
            DropIndex("Catalog.Client", "IDX_BussinessName");
            DropIndex("Catalog.Client", "IDX_Code");
            DropIndex("Common.State", "IDX_Name");
            DropIndex("Common.City", new[] { "StateId" });
            DropIndex("Catalog.ProductImage", new[] { "ProductId" });
            DropIndex("Config.CarModel", new[] { "CarMakeId" });
            DropIndex("Config.CarYear", new[] { "CarModelId" });
            DropIndex("Catalog.Compatibility", new[] { "ProductId" });
            DropIndex("Catalog.Compatibility", new[] { "CarYearId" });
            DropIndex("Catalog.Product", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_Code");
            DropIndex("Catalog.Product", new[] { "CategoryId" });
            DropIndex("Operative.ProductInventory", new[] { "BranchId" });
            DropIndex("Operative.ProductInventory", new[] { "ProductId" });
            DropTable("Catalog.Provider");
            DropTable("Catalog.Employee");
            DropTable("Catalog.Client");
            DropTable("Common.State");
            DropTable("Common.City");
            DropTable("Catalog.ProductImage");
            DropTable("Config.CarMake");
            DropTable("Config.CarModel");
            DropTable("Config.CarYear");
            DropTable("Catalog.Compatibility");
            DropTable("Config.Category");
            DropTable("Catalog.Product");
            DropTable("Operative.ProductInventory");
            DropTable("Config.Branch");
        }
    }
}
