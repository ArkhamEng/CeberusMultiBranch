namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Security : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "Inventory.Product", newSchema: "Catalog");
            DropForeignKey("Inventory.ProductFile", "ProductId", "Inventory.Product");
            DropIndex("Inventory.ProductFile", new[] { "ProductId" });
            CreateTable(
                "Common.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "Common.SubCategory",
                c => new
                    {
                        SubCategoryId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Name = c.String(),
                        UnitMeasure = c.String(),
                    })
                .PrimaryKey(t => t.SubCategoryId)
                .ForeignKey("Common.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
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
                "Common.City",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        StateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("Common.State", t => t.StateId, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "IDX_Name")
                .Index(t => t.StateId);
            
            CreateTable(
                "Common.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.StateId)
                .Index(t => t.Name, unique: true, name: "IDX_Name");
            
            CreateTable(
                "Catalog.Employee",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false),
                        BusinessName = c.String(maxLength: 50),
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
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("Common.City", t => t.CityId, cascadeDelete: true)
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.BusinessName, name: "IDX_BussinessName")
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
            
            AddColumn("Catalog.Product", "SubCategoryId", c => c.Int(nullable: false));
            AddColumn("Catalog.Product", "StorePercentage", c => c.Int(nullable: false));
            AddColumn("Catalog.Product", "DealerPercentage", c => c.Int(nullable: false));
            AddColumn("Catalog.Product", "StorePrice", c => c.Double(nullable: false));
            AddColumn("Catalog.Product", "DealerPrice", c => c.Double(nullable: false));
            AlterColumn("Catalog.Product", "Code", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("Catalog.Product", "Name", c => c.String(maxLength: 20));
            CreateIndex("Catalog.Product", "SubCategoryId");
            CreateIndex("Catalog.Product", "Code", unique: true, name: "IDX_Code");
            CreateIndex("Catalog.Product", "Name", unique: true, name: "IDX_Name");
            CreateIndex("Inventory.ProductInBranch", "ProductId");
            AddForeignKey("Catalog.Product", "SubCategoryId", "Common.SubCategory", "SubCategoryId", cascadeDelete: true);
            AddForeignKey("Inventory.ProductInBranch", "ProductId", "Catalog.Product", "ProductId", cascadeDelete: true);
            DropTable("Inventory.ProductFile");
        }
        
        public override void Down()
        {
            CreateTable(
                "Inventory.ProductFile",
                c => new
                    {
                        ProductFileId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(),
                        File = c.Binary(),
                        Type = c.String(),
                        Size = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProductFileId);
            
            DropForeignKey("Catalog.Provider", "CityId", "Common.City");
            DropForeignKey("Inventory.ProductInBranch", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Employee", "CityId", "Common.City");
            DropForeignKey("Catalog.Client", "CityId", "Common.City");
            DropForeignKey("Common.City", "StateId", "Common.State");
            DropForeignKey("Catalog.Product", "SubCategoryId", "Common.SubCategory");
            DropForeignKey("Inventory.ProductImage", "ProductId", "Catalog.Product");
            DropForeignKey("Common.SubCategory", "CategoryId", "Common.Category");
            DropIndex("Catalog.Provider", new[] { "CityId" });
            DropIndex("Catalog.Provider", "IDX_Phone");
            DropIndex("Catalog.Provider", "IDX_Email");
            DropIndex("Catalog.Provider", "IDX_FTR");
            DropIndex("Catalog.Provider", "IDX_BussinessName");
            DropIndex("Catalog.Provider", "IDX_Code");
            DropIndex("Inventory.ProductInBranch", new[] { "ProductId" });
            DropIndex("Catalog.Employee", new[] { "CityId" });
            DropIndex("Catalog.Employee", "IDX_Phone");
            DropIndex("Catalog.Employee", "IDX_Email");
            DropIndex("Catalog.Employee", "IDX_FTR");
            DropIndex("Catalog.Employee", "IDX_BussinessName");
            DropIndex("Catalog.Employee", "IDX_Code");
            DropIndex("Common.State", "IDX_Name");
            DropIndex("Common.City", new[] { "StateId" });
            DropIndex("Common.City", "IDX_Name");
            DropIndex("Catalog.Client", new[] { "CityId" });
            DropIndex("Catalog.Client", "IDX_Phone");
            DropIndex("Catalog.Client", "IDX_Email");
            DropIndex("Catalog.Client", "IDX_FTR");
            DropIndex("Catalog.Client", "IDX_BussinessName");
            DropIndex("Catalog.Client", "IDX_Code");
            DropIndex("Inventory.ProductImage", new[] { "ProductId" });
            DropIndex("Catalog.Product", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_Code");
            DropIndex("Catalog.Product", new[] { "SubCategoryId" });
            DropIndex("Common.SubCategory", new[] { "CategoryId" });
            AlterColumn("Catalog.Product", "Name", c => c.String());
            AlterColumn("Catalog.Product", "Code", c => c.String());
            DropColumn("Catalog.Product", "DealerPrice");
            DropColumn("Catalog.Product", "StorePrice");
            DropColumn("Catalog.Product", "DealerPercentage");
            DropColumn("Catalog.Product", "StorePercentage");
            DropColumn("Catalog.Product", "SubCategoryId");
            DropTable("Catalog.Provider");
            DropTable("Catalog.Employee");
            DropTable("Common.State");
            DropTable("Common.City");
            DropTable("Catalog.Client");
            DropTable("Inventory.ProductImage");
            DropTable("Common.SubCategory");
            DropTable("Common.Category");
            CreateIndex("Inventory.ProductFile", "ProductId");
            AddForeignKey("Inventory.ProductFile", "ProductId", "Inventory.Product", "ProductId", cascadeDelete: true);
            MoveTable(name: "Catalog.Product", newSchema: "Inventory");
        }
    }
}
