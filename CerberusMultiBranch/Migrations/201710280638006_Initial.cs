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
                "Operative.BranchProduct",
                c => new
                    {
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Stock = c.Double(nullable: false),
                        LastStock = c.Double(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.BranchId, t.ProductId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Catalog.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        PartSystemId = c.Int(),
                        Code = c.String(nullable: false, maxLength: 30),
                        Name = c.String(maxLength: 200),
                        MinQuantity = c.Double(nullable: false),
                        BarCode = c.String(),
                        BuyPrice = c.Double(nullable: false),
                        StorePercentage = c.Int(nullable: false),
                        DealerPercentage = c.Int(nullable: false),
                        WholesalerPercentage = c.Int(nullable: false),
                        StorePrice = c.Double(nullable: false),
                        WholesalerPrice = c.Double(nullable: false),
                        DealerPrice = c.Double(nullable: false),
                        TradeMark = c.String(maxLength: 50),
                        Unit = c.String(maxLength: 20),
                        Row = c.String(maxLength: 30),
                        Ledge = c.String(maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("Config.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("Config.PartSystem", t => t.PartSystemId)
                .Index(t => t.CategoryId, name: "IDX_CategoryId")
                .Index(t => t.PartSystemId, name: "IDX_PartSystemId")
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.Name, name: "IDX_Name");
            
            CreateTable(
                "Config.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.CategoryId)
                .Index(t => t.Name, name: "IDX_Name");
            
            CreateTable(
                "Catalog.Compatibility",
                c => new
                    {
                        CarYearId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CarYearId, t.ProductId })
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
                .Index(t => t.CarModelId, name: "IDX_CarModelId");
            
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
                .Index(t => t.CarMakeId, name: "IDX_CarMakeId");
            
            CreateTable(
                "Config.CarMake",
                c => new
                    {
                        CarMakeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CarMakeId);
            
            CreateTable(
                "Catalog.Equivalence",
                c => new
                    {
                        EquivalenceId = c.Int(nullable: false, identity: true),
                        ProviderId = c.Int(nullable: false),
                        Code = c.String(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EquivalenceId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Catalog.ProductImage",
                c => new
                    {
                        ProductImageId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 80),
                        Path = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 30),
                        Size = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductImageId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId, name: "IDX_ProductId");
            
            CreateTable(
                "Config.PartSystem",
                c => new
                    {
                        PartSystemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PartSystemId);
            
            CreateTable(
                "Operative.TransactionDetail",
                c => new
                    {
                        TransactionId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.TransactionId, t.ProductId })
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Operative.Transaction", t => t.TransactionId, cascadeDelete: true)
                .Index(t => t.TransactionId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Operative.Transaction",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        TotalAmount = c.Double(nullable: false),
                        PaymentType = c.Int(),
                        TransactionDate = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        IsPayed = c.Boolean(nullable: false),
                        ProviderId = c.Int(),
                        Bill = c.String(maxLength: 30),
                        Expiration = c.DateTime(),
                        Inventoried = c.Boolean(),
                        ClientId = c.Int(),
                        Folio = c.String(maxLength: 30),
                        Compleated = c.Boolean(),
                        ComPer = c.Int(),
                        ComAmount = c.Double(),
                        OriginBranchId = c.Int(),
                        AuthUser = c.String(),
                        AuthDate = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Catalog.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("Config.Branch", t => t.OriginBranchId, cascadeDelete: false)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .Index(t => t.BranchId, name: "IDX_BranchId")
                .Index(t => t.UserId, name: "IDX_UserId")
                .Index(t => t.TransactionDate, name: "IDX_TransactionDate")
                .Index(t => t.IsPayed, name: "IDX_IsPayed")
                .Index(t => t.ProviderId)
                .Index(t => t.Inventoried, name: "IDX_Inventoried")
                .Index(t => t.ClientId)
                .Index(t => t.OriginBranchId);
            
            CreateTable(
                "Operative.Payment",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        TransactionId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        PaymentType = c.Int(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("Operative.Transaction", t => t.TransactionId, cascadeDelete: true)
                .Index(t => t.TransactionId);
            
            CreateTable(
                "Security.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PicturePath = c.String(),
                        ComissionForSale = c.Int(nullable: false),
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
                "Catalog.Employee",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false, maxLength: 100),
                        FTR = c.String(maxLength: 15),
                        NSS = c.String(maxLength: 15),
                        Address = c.String(nullable: false, maxLength: 100),
                        ZipCode = c.String(maxLength: 6),
                        Entrance = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 30),
                        Phone = c.String(maxLength: 20),
                        EmergencyPhone = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                        Picture = c.Binary(),
                        PictureType = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("Config.City", t => t.CityId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId)
                .Index(t => t.CityId, name: "IDX_CityId")
                .Index(t => t.UserId, unique: true, name: "IDX_UserId")
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.Name, unique: true, name: "IDX_Name")
                .Index(t => t.Email, unique: true, name: "IDX_Email");
            
            CreateTable(
                "Config.City",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        StateId = c.Int(nullable: false),
                        Code = c.String(maxLength: 15),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("Config.State", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId, name: "IDX_StateId");
            
            CreateTable(
                "Config.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 15),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShorName = c.String(maxLength: 10),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateId)
                .Index(t => t.Name, unique: true, name: "IDX_Name");
            
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
            
            CreateTable(
                "Security.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Security.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Config.UserBranch",
                c => new
                    {
                        BranchId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.BranchId, t.UserId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.UserId);
            
            CreateTable(
                "Catalog.Client",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 100),
                        BusinessName = c.String(maxLength: 100),
                        LegalRepresentative = c.String(maxLength: 100),
                        FTR = c.String(maxLength: 15),
                        TaxAddress = c.String(),
                        Address = c.String(nullable: false, maxLength: 150),
                        ZipCode = c.String(maxLength: 10),
                        Entrance = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 30),
                        Phone = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("Config.City", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.Code, unique: true, name: "IDX_Code");
            
            CreateTable(
                "Catalog.Provider",
                c => new
                    {
                        ProviderId = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 100),
                        BusinessName = c.String(maxLength: 100),
                        WebSite = c.String(maxLength: 30),
                        FTR = c.String(maxLength: 15),
                        Address = c.String(nullable: false, maxLength: 150),
                        ZipCode = c.String(maxLength: 10),
                        Email = c.String(maxLength: 30),
                        Phone = c.String(nullable: false, maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UdpUser = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ProviderId)
                .ForeignKey("Config.City", t => t.CityId, cascadeDelete: false)
                .Index(t => t.CityId)
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.Name, name: "IDX_Name")
                .Index(t => t.Email, name: "IDX_Email");
            
            CreateTable(
                "Operative.CashRegister",
                c => new
                    {
                        CashRegisterId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        UserOpen = c.String(maxLength: 50),
                        UserClose = c.String(maxLength: 50),
                        InitialAmount = c.Double(nullable: false),
                        FinalAmount = c.Double(nullable: false),
                        CloseComment = c.String(maxLength: 100),
                        OpeningDate = c.DateTime(nullable: false),
                        ClosingDate = c.DateTime(),
                        IsOpen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CashRegisterId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .Index(t => t.BranchId);
            
            CreateTable(
                "Operative.CashDetail",
                c => new
                    {
                        CashDetailId = c.Int(nullable: false, identity: true),
                        CashRegisterId = c.Int(nullable: false),
                        User = c.String(),
                        Amount = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        Type = c.Int(),
                        SaleFolio = c.String(),
                        Comment = c.String(maxLength: 100),
                        WithdrawalCauseId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CashDetailId)
                .ForeignKey("Operative.CashRegister", t => t.CashRegisterId, cascadeDelete: true)
                .ForeignKey("Operative.WithdrawalCause", t => t.WithdrawalCauseId, cascadeDelete: true)
                .Index(t => t.CashRegisterId)
                .Index(t => t.WithdrawalCauseId);
            
            CreateTable(
                "Operative.WithdrawalCause",
                c => new
                    {
                        WithdrawalCauseId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserAdd = c.String(),
                        InsDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WithdrawalCauseId);
            
            CreateTable(
                "Catalog.ExternalProduct",
                c => new
                    {
                        ProviderId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 30),
                        Category = c.String(nullable: false, maxLength: 60),
                        Description = c.String(maxLength: 200),
                        Price = c.Double(nullable: false),
                        TradeMark = c.String(maxLength: 50),
                        Unit = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => new { t.ProviderId, t.Code })
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .Index(t => t.ProviderId, name: "IDX_ProviderId")
                .Index(t => t.Code, name: "IDX_Code")
                .Index(t => t.Category, name: "IDX_Category")
                .Index(t => t.Description, name: "IDX_Descripction");
            
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
                "Config.Variable",
                c => new
                    {
                        VariableId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        Value = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.VariableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Security.AspNetUserRoles", "RoleId", "Security.AspNetRoles");
            DropForeignKey("Catalog.ExternalProduct", "ProviderId", "Catalog.Provider");
            DropForeignKey("Operative.CashDetail", "WithdrawalCauseId", "Operative.WithdrawalCause");
            DropForeignKey("Operative.CashDetail", "CashRegisterId", "Operative.CashRegister");
            DropForeignKey("Operative.CashRegister", "BranchId", "Config.Branch");
            DropForeignKey("Operative.BranchProduct", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.TransactionDetail", "TransactionId", "Operative.Transaction");
            DropForeignKey("Operative.Transaction", "ProviderId", "Catalog.Provider");
            DropForeignKey("Catalog.Provider", "CityId", "Config.City");
            DropForeignKey("Operative.Transaction", "OriginBranchId", "Config.Branch");
            DropForeignKey("Operative.Transaction", "ClientId", "Catalog.Client");
            DropForeignKey("Catalog.Client", "CityId", "Config.City");
            DropForeignKey("Config.UserBranch", "UserId", "Security.AspNetUsers");
            DropForeignKey("Config.UserBranch", "BranchId", "Config.Branch");
            DropForeignKey("Operative.Transaction", "UserId", "Security.AspNetUsers");
            DropForeignKey("Security.AspNetUserRoles", "UserId", "Security.AspNetUsers");
            DropForeignKey("Security.AspNetUserLogins", "UserId", "Security.AspNetUsers");
            DropForeignKey("Catalog.Employee", "UserId", "Security.AspNetUsers");
            DropForeignKey("Catalog.Employee", "CityId", "Config.City");
            DropForeignKey("Config.City", "StateId", "Config.State");
            DropForeignKey("Security.AspNetUserClaims", "UserId", "Security.AspNetUsers");
            DropForeignKey("Operative.Payment", "TransactionId", "Operative.Transaction");
            DropForeignKey("Operative.Transaction", "BranchId", "Config.Branch");
            DropForeignKey("Operative.TransactionDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Product", "PartSystemId", "Config.PartSystem");
            DropForeignKey("Catalog.ProductImage", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Equivalence", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Compatibility", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Compatibility", "CarYearId", "Config.CarYear");
            DropForeignKey("Config.CarYear", "CarModelId", "Config.CarModel");
            DropForeignKey("Config.CarModel", "CarMakeId", "Config.CarMake");
            DropForeignKey("Catalog.Product", "CategoryId", "Config.Category");
            DropForeignKey("Operative.BranchProduct", "BranchId", "Config.Branch");
            DropIndex("Security.AspNetRoles", "RoleNameIndex");
            DropIndex("Catalog.ExternalProduct", "IDX_Descripction");
            DropIndex("Catalog.ExternalProduct", "IDX_Category");
            DropIndex("Catalog.ExternalProduct", "IDX_Code");
            DropIndex("Catalog.ExternalProduct", "IDX_ProviderId");
            DropIndex("Operative.CashDetail", new[] { "WithdrawalCauseId" });
            DropIndex("Operative.CashDetail", new[] { "CashRegisterId" });
            DropIndex("Operative.CashRegister", new[] { "BranchId" });
            DropIndex("Catalog.Provider", "IDX_Email");
            DropIndex("Catalog.Provider", "IDX_Name");
            DropIndex("Catalog.Provider", "IDX_Code");
            DropIndex("Catalog.Provider", new[] { "CityId" });
            DropIndex("Catalog.Client", "IDX_Code");
            DropIndex("Catalog.Client", new[] { "CityId" });
            DropIndex("Config.UserBranch", new[] { "UserId" });
            DropIndex("Config.UserBranch", new[] { "BranchId" });
            DropIndex("Security.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("Security.AspNetUserRoles", new[] { "UserId" });
            DropIndex("Security.AspNetUserLogins", new[] { "UserId" });
            DropIndex("Config.State", "IDX_Name");
            DropIndex("Config.City", "IDX_StateId");
            DropIndex("Catalog.Employee", "IDX_Email");
            DropIndex("Catalog.Employee", "IDX_Name");
            DropIndex("Catalog.Employee", "IDX_Code");
            DropIndex("Catalog.Employee", "IDX_UserId");
            DropIndex("Catalog.Employee", "IDX_CityId");
            DropIndex("Security.AspNetUserClaims", new[] { "UserId" });
            DropIndex("Security.AspNetUsers", "UserNameIndex");
            DropIndex("Operative.Payment", new[] { "TransactionId" });
            DropIndex("Operative.Transaction", new[] { "OriginBranchId" });
            DropIndex("Operative.Transaction", new[] { "ClientId" });
            DropIndex("Operative.Transaction", "IDX_Inventoried");
            DropIndex("Operative.Transaction", new[] { "ProviderId" });
            DropIndex("Operative.Transaction", "IDX_IsPayed");
            DropIndex("Operative.Transaction", "IDX_TransactionDate");
            DropIndex("Operative.Transaction", "IDX_UserId");
            DropIndex("Operative.Transaction", "IDX_BranchId");
            DropIndex("Operative.TransactionDetail", new[] { "ProductId" });
            DropIndex("Operative.TransactionDetail", new[] { "TransactionId" });
            DropIndex("Catalog.ProductImage", "IDX_ProductId");
            DropIndex("Catalog.Equivalence", new[] { "ProductId" });
            DropIndex("Config.CarModel", "IDX_CarMakeId");
            DropIndex("Config.CarYear", "IDX_CarModelId");
            DropIndex("Catalog.Compatibility", new[] { "ProductId" });
            DropIndex("Catalog.Compatibility", new[] { "CarYearId" });
            DropIndex("Config.Category", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_Code");
            DropIndex("Catalog.Product", "IDX_PartSystemId");
            DropIndex("Catalog.Product", "IDX_CategoryId");
            DropIndex("Operative.BranchProduct", new[] { "ProductId" });
            DropIndex("Operative.BranchProduct", new[] { "BranchId" });
            DropTable("Config.Variable");
            DropTable("Security.AspNetRoles");
            DropTable("Catalog.ExternalProduct");
            DropTable("Operative.WithdrawalCause");
            DropTable("Operative.CashDetail");
            DropTable("Operative.CashRegister");
            DropTable("Catalog.Provider");
            DropTable("Catalog.Client");
            DropTable("Config.UserBranch");
            DropTable("Security.AspNetUserRoles");
            DropTable("Security.AspNetUserLogins");
            DropTable("Config.State");
            DropTable("Config.City");
            DropTable("Catalog.Employee");
            DropTable("Security.AspNetUserClaims");
            DropTable("Security.AspNetUsers");
            DropTable("Operative.Payment");
            DropTable("Operative.Transaction");
            DropTable("Operative.TransactionDetail");
            DropTable("Config.PartSystem");
            DropTable("Catalog.ProductImage");
            DropTable("Catalog.Equivalence");
            DropTable("Config.CarMake");
            DropTable("Config.CarModel");
            DropTable("Config.CarYear");
            DropTable("Catalog.Compatibility");
            DropTable("Config.Category");
            DropTable("Catalog.Product");
            DropTable("Operative.BranchProduct");
            DropTable("Config.Branch");
        }
    }
}
