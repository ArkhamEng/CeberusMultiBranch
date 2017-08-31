namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Config.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "Catalog.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(maxLength: 20),
                        Description = c.String(),
                        MinQuantity = c.Double(nullable: false),
                        BarCode = c.String(),
                        BuyPrice = c.Double(nullable: false),
                        StorePercentage = c.Int(nullable: false),
                        DealerPercentage = c.Int(nullable: false),
                        WholesalerPercentage = c.Int(nullable: false),
                        StorePrice = c.Double(nullable: false),
                        WholesalerPrice = c.Double(nullable: false),
                        DealerPrice = c.Double(nullable: false),
                        MinimunPrice = c.Double(nullable: false),
                        TradeMark = c.String(),
                        Unit = c.String(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("Config.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.Name, unique: true, name: "IDX_Name");
            
            CreateTable(
                "Inventory.ProductImage",
                c => new
                    {
                        ProductImageId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(),
                        File = c.Binary(),
                        Type = c.String(),
                        Size = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProductImageId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Common.City",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        StateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("Common.State", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
            CreateTable(
                "Common.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShorName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateId)
                .Index(t => t.Name, unique: true, name: "IDX_Name");
            
            CreateTable(
                "Catalog.Client",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false),
                        BusinessName = c.String(maxLength: 50),
                        LegalRepresentative = c.String(maxLength: 50),
                        FTR = c.String(maxLength: 13),
                        TaxAddress = c.String(),
                        Address = c.String(nullable: false),
                        ZipCode = c.String(),
                        Entrance = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 30),
                        Phone = c.String(nullable: false, maxLength: 20),
                        CityId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("Common.City", t => t.CityId, cascadeDelete: true)
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.BusinessName, name: "IDX_BussinessName")
                .Index(t => t.FTR, name: "IDX_FTR")
                .Index(t => t.Email, name: "IDX_Email")
                .Index(t => t.Phone, unique: true, name: "IDX_Phone")
                .Index(t => t.CityId);
            
            CreateTable(
                "Catalog.Employee",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false),
                        FTR = c.String(maxLength: 13),
                        TaxAddress = c.String(),
                        Address = c.String(nullable: false),
                        ZipCode = c.String(),
                        Entrance = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 30),
                        Phone = c.String(nullable: false, maxLength: 20),
                        CityId = c.Int(nullable: false),
                        Picture = c.Binary(),
                        IsActive = c.Boolean(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("Common.City", t => t.CityId, cascadeDelete: true)
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.FTR, name: "IDX_FTR")
                .Index(t => t.Email, name: "IDX_Email")
                .Index(t => t.Phone, unique: true, name: "IDX_Phone")
                .Index(t => t.CityId);
            
            CreateTable(
                "Config.Make",
                c => new
                    {
                        CarMakeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CarMakeId);
            
            CreateTable(
                "Config.CarModel",
                c => new
                    {
                        CarModelId = c.Int(nullable: false, identity: true),
                        MakeId = c.Int(nullable: false),
                        Name = c.String(),
                        Make_CarMakeId = c.Int(),
                    })
                .PrimaryKey(t => t.CarModelId)
                .ForeignKey("Config.Make", t => t.Make_CarMakeId)
                .Index(t => t.Make_CarMakeId);
            
            CreateTable(
                "Inventory.ProductInBranch",
                c => new
                    {
                        ProductInBranchId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProductInBranchId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Catalog.Provider",
                c => new
                    {
                        ProviderId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false),
                        BusinessName = c.String(maxLength: 50),
                        WebSite = c.String(),
                        FTR = c.String(maxLength: 13),
                        Address = c.String(nullable: false),
                        ZipCode = c.String(),
                        Email = c.String(maxLength: 30),
                        Phone = c.String(nullable: false, maxLength: 20),
                        CityId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProviderId)
                .ForeignKey("Common.City", t => t.CityId, cascadeDelete: true)
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.BusinessName, name: "IDX_BussinessName")
                .Index(t => t.FTR, name: "IDX_FTR")
                .Index(t => t.Email, name: "IDX_Email")
                .Index(t => t.Phone, unique: true, name: "IDX_Phone")
                .Index(t => t.CityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Catalog.Provider", "CityId", "Common.City");
            DropForeignKey("Inventory.ProductInBranch", "ProductId", "Catalog.Product");
            DropForeignKey("Config.CarModel", "Make_CarMakeId", "Config.Make");
            DropForeignKey("Catalog.Employee", "CityId", "Common.City");
            DropForeignKey("Catalog.Client", "CityId", "Common.City");
            DropForeignKey("Common.City", "StateId", "Common.State");
            DropForeignKey("Inventory.ProductImage", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Product", "CategoryId", "Config.Category");
            DropIndex("Catalog.Provider", new[] { "CityId" });
            DropIndex("Catalog.Provider", "IDX_Phone");
            DropIndex("Catalog.Provider", "IDX_Email");
            DropIndex("Catalog.Provider", "IDX_FTR");
            DropIndex("Catalog.Provider", "IDX_BussinessName");
            DropIndex("Catalog.Provider", "IDX_Code");
            DropIndex("Inventory.ProductInBranch", new[] { "ProductId" });
            DropIndex("Config.CarModel", new[] { "Make_CarMakeId" });
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
            DropIndex("Inventory.ProductImage", new[] { "ProductId" });
            DropIndex("Catalog.Product", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_Code");
            DropIndex("Catalog.Product", new[] { "CategoryId" });
            DropTable("Catalog.Provider");
            DropTable("Inventory.ProductInBranch");
            DropTable("Config.CarModel");
            DropTable("Config.Make");
            DropTable("Catalog.Employee");
            DropTable("Catalog.Client");
            DropTable("Common.State");
            DropTable("Common.City");
            DropTable("Inventory.ProductImage");
            DropTable("Catalog.Product");
            DropTable("Config.Category");
        }
    }
}
