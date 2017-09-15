namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
            
            CreateTable(
                "Catalog.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(maxLength: 100),
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
                "Config.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "Catalog.Compatibility",
                c => new
                    {
                        CompatibilityId = c.Int(nullable: false, identity: true),
                        CarYearId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompatibilityId)
                .ForeignKey("Config.CarYear", t => t.CarYearId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.CarYearId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Config.CarYear",
                c => new
                    {
                        CarYearId = c.Int(nullable: false, identity: true),
                        CarModelId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarYearId)
                .ForeignKey("Config.CarModel", t => t.CarModelId, cascadeDelete: true)
                .Index(t => t.CarModelId);
            
            CreateTable(
                "Config.CarModel",
                c => new
                    {
                        CarModelId = c.Int(nullable: false, identity: true),
                        CarMakeId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CarModelId)
                .ForeignKey("Config.CarMake", t => t.CarMakeId, cascadeDelete: true)
                .Index(t => t.CarMakeId);
            
            CreateTable(
                "Config.CarMake",
                c => new
                    {
                        CarMakeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CarMakeId);
            
            CreateTable(
                "Catalog.ProductImage",
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
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("Common.City", t => t.CityId, cascadeDelete: true)
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.FTR, name: "IDX_FTR")
                .Index(t => t.Email, name: "IDX_Email")
                .Index(t => t.Phone, unique: true, name: "IDX_Phone")
                .Index(t => t.CityId);
            
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
            
            CreateTable(
                "Security.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "Security.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Security.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Security.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EmployeeId = c.Int(),
                        Picture = c.Binary(),
                        PictureType = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "Security.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Security.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Security.AspNetUserRoles", "UserId", "Security.AspNetUsers");
            DropForeignKey("Security.AspNetUserLogins", "UserId", "Security.AspNetUsers");
            DropForeignKey("Security.AspNetUserClaims", "UserId", "Security.AspNetUsers");
            DropForeignKey("Security.AspNetUserRoles", "RoleId", "Security.AspNetRoles");
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
            DropIndex("Security.AspNetUserLogins", new[] { "UserId" });
            DropIndex("Security.AspNetUserClaims", new[] { "UserId" });
            DropIndex("Security.AspNetUsers", "UserNameIndex");
            DropIndex("Security.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("Security.AspNetUserRoles", new[] { "UserId" });
            DropIndex("Security.AspNetRoles", "RoleNameIndex");
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
            DropTable("Security.AspNetUserLogins");
            DropTable("Security.AspNetUserClaims");
            DropTable("Security.AspNetUsers");
            DropTable("Security.AspNetUserRoles");
            DropTable("Security.AspNetRoles");
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
