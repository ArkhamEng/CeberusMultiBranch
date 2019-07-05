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
                        Code = c.String(maxLength: 4),
                        NoteMemberHtml = c.String(maxLength: 500),
                        NoteLocalHtml = c.String(maxLength: 500),
                        LogoPath = c.String(maxLength: 50),
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
                        Reserved = c.Double(nullable: false),
                        BuyPrice = c.Double(nullable: false),
                        StorePercentage = c.Int(nullable: false),
                        DealerPercentage = c.Int(nullable: false),
                        WholesalerPercentage = c.Int(nullable: false),
                        StorePrice = c.Double(nullable: false),
                        WholesalerPrice = c.Double(nullable: false),
                        DealerPrice = c.Double(nullable: false),
                        Row = c.String(maxLength: 30),
                        Ledge = c.String(maxLength: 30),
                        MaxQuantity = c.Double(nullable: false),
                        MinQuantity = c.Double(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        StockLocked = c.Boolean(nullable: false),
                        LockDate = c.DateTime(),
                        UserLock = c.String(maxLength: 30),
                        UpdUser = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.BranchId, t.ProductId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.ProductId)
                .Index(t => t.LockDate, name: "IDX_LockDate")
                .Index(t => t.UserLock, name: "IDX_UserLock");
            
            CreateTable(
                "Operative.InventoryItem",
                c => new
                    {
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        TrackingItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BranchId, t.ProductId, t.TrackingItemId })
                .ForeignKey("Operative.BranchProduct", t => new { t.BranchId, t.ProductId }, cascadeDelete: true)
                .ForeignKey("Operative.TrackingItem", t => t.TrackingItemId, cascadeDelete: true)
                .Index(t => new { t.BranchId, t.ProductId })
                .Index(t => t.TrackingItemId);
            
            CreateTable(
                "Operative.TrackingItem",
                c => new
                    {
                        TrackingItemId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        SerialNumber = c.String(),
                        IsBatch = c.Boolean(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                    })
                .PrimaryKey(t => t.TrackingItemId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Catalog.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        PartSystemId = c.Int(),
                        Code = c.String(nullable: false, maxLength: 30),
                        Name = c.String(nullable: false, maxLength: 200),
                        MinQuantity = c.Double(nullable: false),
                        MaxQuantity = c.Double(nullable: false),
                        BarCode = c.String(),
                        ProductType = c.Int(nullable: false),
                        TradeMark = c.String(maxLength: 50),
                        Unit = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        StockRequired = c.Boolean(nullable: false),
                        IsTrackable = c.Boolean(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                        LockDate = c.DateTime(),
                        UserLock = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("Config.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("Config.PartSystem", t => t.PartSystemId)
                .Index(t => t.CategoryId)
                .Index(t => t.PartSystemId)
                .Index(t => new { t.Code, t.Name, t.TradeMark }, name: "IDX_Ident_TradeMark")
                .Index(t => new { t.LockDate, t.UserLock }, name: "IDX_Lock");
            
            CreateTable(
                "Config.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        SatCode = c.String(nullable: false, maxLength: 30),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId)
                .Index(t => t.Name, unique: true, name: "IDX_Name")
                .Index(t => t.SatCode, unique: true, name: "IDX_SatCode");
            
            CreateTable(
                "Config.SystemCategory",
                c => new
                    {
                        PartSystemId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.PartSystemId, t.CategoryId })
                .ForeignKey("Config.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("Config.PartSystem", t => t.PartSystemId, cascadeDelete: true)
                .Index(t => t.PartSystemId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "Config.PartSystem",
                c => new
                    {
                        PartSystemId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Commission = c.Int(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PartSystemId)
                .Index(t => t.Name, unique: true, name: "IDX_Name");
            
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
                        Name = c.String(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                        UpdDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CarModelId)
                .ForeignKey("Config.CarMake", t => t.CarMakeId, cascadeDelete: true)
                .Index(t => t.CarMakeId, name: "IDX_CarMakeId");
            
            CreateTable(
                "Config.CarMake",
                c => new
                    {
                        CarMakeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                        UpdDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CarMakeId);
            
            CreateTable(
                "Catalog.Equivalence",
                c => new
                    {
                        EquivalenceId = c.Int(nullable: false, identity: true),
                        ProviderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Code = c.String(maxLength: 30),
                        BuyPrice = c.Double(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.EquivalenceId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .Index(t => new { t.ProviderId, t.ProductId }, name: "IDX_Provider_Product")
                .Index(t => t.Code, name: "IDX_Code");
            
            CreateTable(
                "Catalog.Provider",
                c => new
                    {
                        ProviderId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 100),
                        BusinessName = c.String(maxLength: 100),
                        WebSite = c.String(maxLength: 150),
                        FTR = c.String(nullable: false, maxLength: 15),
                        Email = c.String(maxLength: 100),
                        Email2 = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Phone2 = c.String(maxLength: 20),
                        Phone3 = c.String(maxLength: 20),
                        AgentPhone = c.String(maxLength: 20),
                        Agent = c.String(maxLength: 100),
                        Line = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                        LockEndDate = c.DateTime(),
                        LockUser = c.String(maxLength: 100),
                        CreditLimit = c.Double(nullable: false),
                        DaysToPay = c.Int(nullable: false),
                        Catalog = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProviderId)
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.Name, name: "IDX_Name")
                .Index(t => t.FTR, unique: true, name: "Unq_FTR")
                .Index(t => t.Email, name: "IDX_Email");
            
            CreateTable(
                "Catalog.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressType = c.String(maxLength: 20),
                        Entity = c.String(maxLength: 20),
                        CityId = c.Int(nullable: false),
                        Location = c.String(nullable: false, maxLength: 150),
                        Street = c.String(nullable: false, maxLength: 150),
                        ZipCode = c.String(maxLength: 10),
                        Reference = c.String(maxLength: 250),
                        ClientId = c.Int(),
                        EmployeeId = c.Int(),
                        ProviderId = c.Int(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("Config.City", t => t.CityId, cascadeDelete: true)
                .ForeignKey("Catalog.Client", t => t.ClientId)
                .ForeignKey("Catalog.Employee", t => t.EmployeeId)
                .ForeignKey("Catalog.Provider", t => t.ProviderId)
                .Index(t => t.CityId, name: "IDX_CityId")
                .Index(t => t.ClientId)
                .Index(t => t.EmployeeId)
                .Index(t => t.ProviderId);
            
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
                "Catalog.Client",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 100),
                        BusinessName = c.String(maxLength: 100),
                        FTR = c.String(maxLength: 15),
                        Entrance = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 100),
                        Email2 = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Phone2 = c.String(maxLength: 20),
                        Type = c.Int(nullable: false),
                        PersonType = c.String(),
                        CreditLimit = c.Double(nullable: false),
                        UsedAmount = c.Double(nullable: false),
                        CreditDays = c.Int(nullable: false),
                        CreditComment = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                        LockEndDate = c.DateTime(),
                        LockUser = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ClientId)
                .Index(t => t.Code, unique: true, name: "IDX_Code");
            
            CreateTable(
                "Operative.Sale",
                c => new
                    {
                        SaleId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Folio = c.String(nullable: false, maxLength: 30),
                        ComPer = c.Int(nullable: false),
                        ComAmount = c.Double(nullable: false),
                        SendingType = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Sequential = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        LastStatus = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        TotalAmount = c.Double(nullable: false),
                        TotalTaxedAmount = c.Double(nullable: false),
                        TotalTaxAmount = c.Double(nullable: false),
                        DiscountedAmount = c.Double(nullable: false),
                        DiscountPercentage = c.Double(nullable: false),
                        FinalAmount = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Expiration = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SaleId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.Year, name: "IDX_Year")
                .Index(t => t.Sequential, name: "IDX_Sequential")
                .Index(t => t.BranchId, name: "IDX_BranchId")
                .Index(t => t.TransactionDate, name: "IDX_TransactionDate")
                .Index(t => t.UserId, name: "IDX_UserId")
                .Index(t => t.Status, name: "IDX_Status");
            
            CreateTable(
                "Operative.SaleCreditNote",
                c => new
                    {
                        SaleCreditNoteId = c.Int(nullable: false),
                        Folio = c.String(nullable: false, maxLength: 30),
                        Ident = c.String(maxLength: 15),
                        Amount = c.Double(nullable: false),
                        Year = c.Int(nullable: false),
                        Sequential = c.Int(nullable: false),
                        User = c.String(maxLength: 30),
                        RegisterDate = c.DateTime(nullable: false),
                        ExplirationDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleCreditNoteId, t.Folio })
                .ForeignKey("Operative.Sale", t => t.SaleCreditNoteId, cascadeDelete: true)
                .Index(t => t.SaleCreditNoteId)
                .Index(t => new { t.Folio, t.Ident, t.IsActive }, name: "IDX_Identifier_Active")
                .Index(t => new { t.Year, t.Sequential }, name: "IDX_Sequential");
            
            CreateTable(
                "Operative.SaleDetail",
                c => new
                    {
                        SaleId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Commission = c.Double(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        ParentId = c.Int(),
                        Refund = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                        Price = c.Double(nullable: false),
                        TaxPercentage = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        TaxedPrice = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        TaxedAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleId, t.ProductId })
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Operative.Sale", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId)
                .Index(t => t.ProductId)
                .Index(t => t.InsDate, name: "IDX_InsDate");
            
            CreateTable(
                "Operative.SaleHistory",
                c => new
                    {
                        SaleHistoryId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        User = c.String(),
                        Comment = c.String(),
                        Status = c.String(),
                        InsDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleHistoryId)
                .ForeignKey("Operative.Sale", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId);
            
            CreateTable(
                "Finances.SalePayment",
                c => new
                    {
                        SalePaymentId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(),
                        ReceivableId = c.Int(),
                        Amount = c.Double(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Reference = c.String(maxLength: 100),
                        UpdUser = c.String(maxLength: 100),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SalePaymentId)
                .ForeignKey("Finances.Receivable", t => t.ReceivableId)
                .ForeignKey("Operative.Sale", t => t.SaleId)
                .Index(t => t.SaleId)
                .Index(t => t.ReceivableId);
            
            CreateTable(
                "Finances.Receivable",
                c => new
                    {
                        ReceivableId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        InitialAmount = c.Double(nullable: false),
                        CurrentAmount = c.Double(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Period = c.Int(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.ReceivableId)
                .ForeignKey("Operative.Sale", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId);
            
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
                        JobPositionId = c.Int(),
                        UserId = c.String(maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false, maxLength: 100),
                        FTR = c.String(maxLength: 15),
                        NSS = c.String(maxLength: 15),
                        Entrance = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        EmergencyPhone = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                        LockEndDate = c.DateTime(),
                        LockUser = c.String(maxLength: 100),
                        Picture = c.Binary(),
                        PictureType = c.String(maxLength: 20),
                        ComissionForSale = c.Int(nullable: false),
                        Salary = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("Config.JobPosition", t => t.JobPositionId)
                .ForeignKey("Security.AspNetUsers", t => t.UserId)
                .Index(t => t.JobPositionId)
                .Index(t => t.UserId, name: "IDX_UserId")
                .Index(t => t.Code, unique: true, name: "IDX_Code")
                .Index(t => t.Name, unique: true, name: "IDX_Name");
            
            CreateTable(
                "Config.JobPosition",
                c => new
                    {
                        JobPositionId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        UpdUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobPositionId);
            
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
                "Catalog.ExternalProduct",
                c => new
                    {
                        ProviderId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 30),
                        Description = c.String(maxLength: 300),
                        Price = c.Double(nullable: false),
                        TradeMark = c.String(maxLength: 50),
                        Unit = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => new { t.ProviderId, t.Code })
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .Index(t => t.ProviderId, name: "IDX_ProviderId")
                .Index(t => t.Code, name: "IDX_Code")
                .Index(t => t.Description, name: "IDX_Descripction");
            
            CreateTable(
                "Purchasing.PurchaseItem",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        TotalLine = c.Double(nullable: false),
                        PurchaseType_PurchaseTypeId = c.Int(),
                    })
                .PrimaryKey(t => new { t.UserId, t.BranchId, t.ProductId, t.ProviderId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .ForeignKey("Purchasing.PurchaseType", t => t.PurchaseType_PurchaseTypeId)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BranchId)
                .Index(t => t.ProductId)
                .Index(t => t.ProviderId)
                .Index(t => t.PurchaseType_PurchaseTypeId);
            
            CreateTable(
                "Purchasing.PurchaseType",
                c => new
                    {
                        PurchaseTypeId = c.Int(nullable: false),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseTypeId);
            
            CreateTable(
                "Purchasing.PurchaseOrder",
                c => new
                    {
                        PurchaseOrderId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        Folio = c.String(),
                        Bill = c.String(),
                        PurchaseStatusId = c.Int(nullable: false),
                        PurchaseTypeId = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        OrderDate = c.DateTime(),
                        DueDate = c.DateTime(),
                        ShipDate = c.DateTime(),
                        DeliveryDate = c.DateTime(),
                        SubTotal = c.Double(nullable: false),
                        TaxRate = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        DaysToPay = c.Int(nullable: false),
                        Freight = c.Double(nullable: false),
                        Insurance = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                        Comment = c.String(),
                        Year = c.Int(nullable: false),
                        Sequential = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .ForeignKey("Purchasing.PurchaseStatus", t => t.PurchaseStatusId, cascadeDelete: true)
                .ForeignKey("Purchasing.PurchaseType", t => t.PurchaseTypeId, cascadeDelete: true)
                .ForeignKey("Catalog.ShipMethod", t => t.ShipMethodId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.ProviderId)
                .Index(t => t.PurchaseStatusId)
                .Index(t => t.PurchaseTypeId)
                .Index(t => t.ShipMethodId);
            
            CreateTable(
                "Purchasing.PurchaseOrderDetail",
                c => new
                    {
                        PurchaseOrderDetailId = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        OrderQty = c.Double(nullable: false),
                        ReceivedQty = c.Double(nullable: false),
                        ComplementQty = c.Double(nullable: false),
                        StockedQty = c.Double(nullable: false),
                        Comment = c.String(),
                        IsCompleated = c.Boolean(nullable: false),
                        LineTotal = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseOrderDetailId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Purchasing.PurchaseOrder", t => t.PurchaseOrderId, cascadeDelete: true)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Operative.StockMovement",
                c => new
                    {
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        StockMovementId = c.Int(nullable: false, identity: true),
                        TrackingItemId = c.Int(),
                        PurchaseOrderDetailId = c.Int(),
                        Quantity = c.Double(nullable: false),
                        MovementDate = c.DateTime(nullable: false),
                        User = c.String(),
                        MovementType = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.StockMovementId)
                .ForeignKey("Operative.BranchProduct", t => new { t.BranchId, t.ProductId }, cascadeDelete: true)
                .ForeignKey("Purchasing.PurchaseOrderDetail", t => t.PurchaseOrderDetailId)
                .ForeignKey("Operative.TrackingItem", t => t.TrackingItemId)
                .Index(t => new { t.BranchId, t.ProductId })
                .Index(t => t.TrackingItemId)
                .Index(t => t.PurchaseOrderDetailId);
            
            CreateTable(
                "Purchasing.PurchaseOrderHistory",
                c => new
                    {
                        PurchaseOrderHistoryId = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        Comment = c.String(),
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
            
            CreateTable(
                "Purchasing.Purchase",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        ProviderId = c.Int(nullable: false),
                        PurchaseOrderId = c.Int(),
                        Bill = c.String(nullable: false, maxLength: 30),
                        Folio = c.String(maxLength: 20),
                        Freight = c.Double(nullable: false),
                        Insurance = c.Double(nullable: false),
                        BranchId = c.Int(nullable: false),
                        LastStatus = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        TotalAmount = c.Double(nullable: false),
                        TotalTaxedAmount = c.Double(nullable: false),
                        TotalTaxAmount = c.Double(nullable: false),
                        DiscountedAmount = c.Double(nullable: false),
                        DiscountPercentage = c.Double(nullable: false),
                        FinalAmount = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Expiration = c.DateTime(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Provider", t => t.ProviderId, cascadeDelete: true)
                .ForeignKey("Purchasing.PurchaseOrder", t => t.PurchaseOrderId)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProviderId)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.BranchId, name: "IDX_BranchId")
                .Index(t => t.TransactionDate, name: "IDX_TransactionDate")
                .Index(t => t.UserId, name: "IDX_UserId")
                .Index(t => t.Status, name: "IDX_Status");
            
            CreateTable(
                "Purchasing.PurchaseDetail",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        BranchId = c.Int(),
                        Received = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stocked = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rejected = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Double(nullable: false),
                        TaxPercentage = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        TaxedPrice = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        TaxedAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseId, t.ProductId })
                .ForeignKey("Config.Branch", t => t.BranchId)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Purchasing.Purchase", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId)
                .Index(t => t.ProductId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "Finances.PurchaseDiscount",
                c => new
                    {
                        PurchaseDiscountId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        DiscountAmount = c.Double(nullable: false),
                        DiscountPercentage = c.Double(nullable: false),
                        Comment = c.String(),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.PurchaseDiscountId)
                .ForeignKey("Purchasing.Purchase", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId);
            
            CreateTable(
                "Finances.PurchasePayment",
                c => new
                    {
                        PurchasePaymentId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Reference = c.String(maxLength: 100),
                        UpdUser = c.String(maxLength: 100),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchasePaymentId)
                .ForeignKey("Purchasing.Purchase", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId);
            
            CreateTable(
                "Purchasing.PurchaseStatus",
                c => new
                    {
                        PurchaseStatusId = c.Int(nullable: false),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseStatusId);
            
            CreateTable(
                "Catalog.ShipMethod",
                c => new
                    {
                        ShipMethodId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 50),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(),
                    })
                .PrimaryKey(t => t.ShipMethodId);
            
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
                "Catalog.PackageDetail",
                c => new
                    {
                        PackageDetailId = c.Int(nullable: false, identity: true),
                        PackageId = c.Int(nullable: false),
                        DetailtId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PackageDetailId)
                .ForeignKey("Catalog.Product", t => t.DetailtId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.PackageId, cascadeDelete: true)
                .Index(t => t.PackageId)
                .Index(t => t.DetailtId);
            
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
                        Type = c.Int(nullable: false),
                        SaleFolio = c.String(),
                        Comment = c.String(nullable: false, maxLength: 100),
                        WithdrawalCauseId = c.Int(),
                        DetailType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CashDetailId)
                .ForeignKey("Operative.CashRegister", t => t.CashRegisterId, cascadeDelete: true)
                .ForeignKey("Operative.WithdrawalCause", t => t.WithdrawalCauseId)
                .Index(t => t.CashRegisterId)
                .Index(t => t.WithdrawalCauseId);
            
            CreateTable(
                "Operative.WithdrawalCause",
                c => new
                    {
                        WithdrawalCauseId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.WithdrawalCauseId);
            
            CreateTable(
                "Operative.BudgetDetail",
                c => new
                    {
                        BudgetId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        TaxPercentage = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        TaxedPrice = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        TaxedAmount = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.BudgetId, t.ProductId })
                .ForeignKey("Operative.Budget", t => t.BudgetId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BudgetId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Operative.Budget",
                c => new
                    {
                        BudgetId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        BudgetDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        UserName = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.BudgetId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "Operative.CreditNoteHistory",
                c => new
                    {
                        CreditNoteHistoryId = c.Int(nullable: false, identity: true),
                        SaleCreditNoteId = c.Int(nullable: false),
                        Folio = c.String(),
                        Amount = c.Double(nullable: false),
                        User = c.String(maxLength: 30),
                        ChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CreditNoteHistoryId);
            
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
                "Operative.ShoppingCart",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        TaxPercentage = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        TaxedPrice = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        TaxedAmount = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        BudgetId = c.Int(),
                    })
                .PrimaryKey(t => new { t.UserId, t.BranchId, t.ProductId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BranchId)
                .Index(t => t.ProductId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "Catalog.TempExternalProduct",
                c => new
                    {
                        ProviderId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        TradeMark = c.String(),
                        Unit = c.String(),
                    })
                .PrimaryKey(t => new { t.ProviderId, t.Code })
                .Index(t => t.ProviderId, name: "IDX_ProviderId")
                .Index(t => t.Code, name: "IDX_Code");
            
            CreateTable(
                "Config.Variable",
                c => new
                    {
                        VariableId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.VariableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.ShoppingCart", "UserId", "Security.AspNetUsers");
            DropForeignKey("Operative.ShoppingCart", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.ShoppingCart", "ClientId", "Catalog.Client");
            DropForeignKey("Operative.ShoppingCart", "BranchId", "Config.Branch");
            DropForeignKey("Security.AspNetUserRoles", "RoleId", "Security.AspNetRoles");
            DropForeignKey("Operative.BudgetDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.BudgetDetail", "BudgetId", "Operative.Budget");
            DropForeignKey("Operative.Budget", "ClientId", "Catalog.Client");
            DropForeignKey("Operative.Budget", "BranchId", "Config.Branch");
            DropForeignKey("Operative.CashDetail", "WithdrawalCauseId", "Operative.WithdrawalCause");
            DropForeignKey("Operative.CashDetail", "CashRegisterId", "Operative.CashRegister");
            DropForeignKey("Operative.CashRegister", "BranchId", "Config.Branch");
            DropForeignKey("Operative.BranchProduct", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.InventoryItem", "TrackingItemId", "Operative.TrackingItem");
            DropForeignKey("Operative.TrackingItem", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.PackageDetail", "PackageId", "Catalog.Product");
            DropForeignKey("Catalog.PackageDetail", "DetailtId", "Catalog.Product");
            DropForeignKey("Catalog.ProductImage", "ProductId", "Catalog.Product");
            DropForeignKey("Purchasing.PurchaseItem", "UserId", "Security.AspNetUsers");
            DropForeignKey("Purchasing.PurchaseOrder", "ShipMethodId", "Catalog.ShipMethod");
            DropForeignKey("Purchasing.PurchaseOrder", "PurchaseTypeId", "Purchasing.PurchaseType");
            DropForeignKey("Purchasing.PurchaseOrder", "PurchaseStatusId", "Purchasing.PurchaseStatus");
            DropForeignKey("Purchasing.Purchase", "UserId", "Security.AspNetUsers");
            DropForeignKey("Finances.PurchasePayment", "PurchaseId", "Purchasing.Purchase");
            DropForeignKey("Purchasing.Purchase", "PurchaseOrderId", "Purchasing.PurchaseOrder");
            DropForeignKey("Finances.PurchaseDiscount", "PurchaseId", "Purchasing.Purchase");
            DropForeignKey("Purchasing.PurchaseDetail", "PurchaseId", "Purchasing.Purchase");
            DropForeignKey("Purchasing.PurchaseDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Purchasing.PurchaseDetail", "BranchId", "Config.Branch");
            DropForeignKey("Purchasing.Purchase", "ProviderId", "Catalog.Provider");
            DropForeignKey("Purchasing.Purchase", "BranchId", "Config.Branch");
            DropForeignKey("Purchasing.PurchaseOrderHistory", "PurchaseOrderId", "Purchasing.PurchaseOrder");
            DropForeignKey("Operative.StockMovement", "TrackingItemId", "Operative.TrackingItem");
            DropForeignKey("Operative.StockMovement", "PurchaseOrderDetailId", "Purchasing.PurchaseOrderDetail");
            DropForeignKey("Operative.StockMovement", new[] { "BranchId", "ProductId" }, "Operative.BranchProduct");
            DropForeignKey("Purchasing.PurchaseOrderDetail", "PurchaseOrderId", "Purchasing.PurchaseOrder");
            DropForeignKey("Purchasing.PurchaseOrderDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Purchasing.PurchaseOrder", "ProviderId", "Catalog.Provider");
            DropForeignKey("Purchasing.PurchaseOrder", "BranchId", "Config.Branch");
            DropForeignKey("Purchasing.PurchaseItem", "PurchaseType_PurchaseTypeId", "Purchasing.PurchaseType");
            DropForeignKey("Purchasing.PurchaseItem", "ProviderId", "Catalog.Provider");
            DropForeignKey("Purchasing.PurchaseItem", "ProductId", "Catalog.Product");
            DropForeignKey("Purchasing.PurchaseItem", "BranchId", "Config.Branch");
            DropForeignKey("Catalog.ExternalProduct", "ProviderId", "Catalog.Provider");
            DropForeignKey("Catalog.Equivalence", "ProviderId", "Catalog.Provider");
            DropForeignKey("Catalog.Address", "ProviderId", "Catalog.Provider");
            DropForeignKey("Catalog.Address", "EmployeeId", "Catalog.Employee");
            DropForeignKey("Catalog.Address", "ClientId", "Catalog.Client");
            DropForeignKey("Operative.Sale", "UserId", "Security.AspNetUsers");
            DropForeignKey("Config.UserBranch", "UserId", "Security.AspNetUsers");
            DropForeignKey("Config.UserBranch", "BranchId", "Config.Branch");
            DropForeignKey("Security.AspNetUserRoles", "UserId", "Security.AspNetUsers");
            DropForeignKey("Security.AspNetUserLogins", "UserId", "Security.AspNetUsers");
            DropForeignKey("Catalog.Employee", "UserId", "Security.AspNetUsers");
            DropForeignKey("Catalog.Employee", "JobPositionId", "Config.JobPosition");
            DropForeignKey("Security.AspNetUserClaims", "UserId", "Security.AspNetUsers");
            DropForeignKey("Finances.SalePayment", "SaleId", "Operative.Sale");
            DropForeignKey("Finances.SalePayment", "ReceivableId", "Finances.Receivable");
            DropForeignKey("Finances.Receivable", "SaleId", "Operative.Sale");
            DropForeignKey("Operative.SaleHistory", "SaleId", "Operative.Sale");
            DropForeignKey("Operative.SaleDetail", "SaleId", "Operative.Sale");
            DropForeignKey("Operative.SaleDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.SaleCreditNote", "SaleCreditNoteId", "Operative.Sale");
            DropForeignKey("Operative.Sale", "ClientId", "Catalog.Client");
            DropForeignKey("Operative.Sale", "BranchId", "Config.Branch");
            DropForeignKey("Catalog.Address", "CityId", "Config.City");
            DropForeignKey("Config.City", "StateId", "Config.State");
            DropForeignKey("Catalog.Equivalence", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Compatibility", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Compatibility", "CarYearId", "Config.CarYear");
            DropForeignKey("Config.CarYear", "CarModelId", "Config.CarModel");
            DropForeignKey("Config.CarModel", "CarMakeId", "Config.CarMake");
            DropForeignKey("Config.SystemCategory", "PartSystemId", "Config.PartSystem");
            DropForeignKey("Catalog.Product", "PartSystemId", "Config.PartSystem");
            DropForeignKey("Config.SystemCategory", "CategoryId", "Config.Category");
            DropForeignKey("Catalog.Product", "CategoryId", "Config.Category");
            DropForeignKey("Operative.InventoryItem", new[] { "BranchId", "ProductId" }, "Operative.BranchProduct");
            DropForeignKey("Operative.BranchProduct", "BranchId", "Config.Branch");
            DropIndex("Catalog.TempExternalProduct", "IDX_Code");
            DropIndex("Catalog.TempExternalProduct", "IDX_ProviderId");
            DropIndex("Operative.ShoppingCart", new[] { "ClientId" });
            DropIndex("Operative.ShoppingCart", new[] { "ProductId" });
            DropIndex("Operative.ShoppingCart", new[] { "BranchId" });
            DropIndex("Operative.ShoppingCart", new[] { "UserId" });
            DropIndex("Security.AspNetRoles", "RoleNameIndex");
            DropIndex("Operative.Budget", new[] { "ClientId" });
            DropIndex("Operative.Budget", new[] { "BranchId" });
            DropIndex("Operative.BudgetDetail", new[] { "ProductId" });
            DropIndex("Operative.BudgetDetail", new[] { "BudgetId" });
            DropIndex("Operative.CashDetail", new[] { "WithdrawalCauseId" });
            DropIndex("Operative.CashDetail", new[] { "CashRegisterId" });
            DropIndex("Operative.CashRegister", new[] { "BranchId" });
            DropIndex("Catalog.PackageDetail", new[] { "DetailtId" });
            DropIndex("Catalog.PackageDetail", new[] { "PackageId" });
            DropIndex("Catalog.ProductImage", "IDX_ProductId");
            DropIndex("Finances.PurchasePayment", new[] { "PurchaseId" });
            DropIndex("Finances.PurchaseDiscount", new[] { "PurchaseId" });
            DropIndex("Purchasing.PurchaseDetail", new[] { "BranchId" });
            DropIndex("Purchasing.PurchaseDetail", new[] { "ProductId" });
            DropIndex("Purchasing.PurchaseDetail", new[] { "PurchaseId" });
            DropIndex("Purchasing.Purchase", "IDX_Status");
            DropIndex("Purchasing.Purchase", "IDX_UserId");
            DropIndex("Purchasing.Purchase", "IDX_TransactionDate");
            DropIndex("Purchasing.Purchase", "IDX_BranchId");
            DropIndex("Purchasing.Purchase", new[] { "PurchaseOrderId" });
            DropIndex("Purchasing.Purchase", new[] { "ProviderId" });
            DropIndex("Purchasing.PurchaseOrderHistory", new[] { "PurchaseOrderId" });
            DropIndex("Operative.StockMovement", new[] { "PurchaseOrderDetailId" });
            DropIndex("Operative.StockMovement", new[] { "TrackingItemId" });
            DropIndex("Operative.StockMovement", new[] { "BranchId", "ProductId" });
            DropIndex("Purchasing.PurchaseOrderDetail", new[] { "ProductId" });
            DropIndex("Purchasing.PurchaseOrderDetail", new[] { "PurchaseOrderId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "ShipMethodId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "PurchaseTypeId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "PurchaseStatusId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "ProviderId" });
            DropIndex("Purchasing.PurchaseOrder", new[] { "BranchId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "PurchaseType_PurchaseTypeId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "ProviderId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "ProductId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "BranchId" });
            DropIndex("Purchasing.PurchaseItem", new[] { "UserId" });
            DropIndex("Catalog.ExternalProduct", "IDX_Descripction");
            DropIndex("Catalog.ExternalProduct", "IDX_Code");
            DropIndex("Catalog.ExternalProduct", "IDX_ProviderId");
            DropIndex("Config.UserBranch", new[] { "UserId" });
            DropIndex("Config.UserBranch", new[] { "BranchId" });
            DropIndex("Security.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("Security.AspNetUserRoles", new[] { "UserId" });
            DropIndex("Security.AspNetUserLogins", new[] { "UserId" });
            DropIndex("Catalog.Employee", "IDX_Name");
            DropIndex("Catalog.Employee", "IDX_Code");
            DropIndex("Catalog.Employee", "IDX_UserId");
            DropIndex("Catalog.Employee", new[] { "JobPositionId" });
            DropIndex("Security.AspNetUserClaims", new[] { "UserId" });
            DropIndex("Security.AspNetUsers", "UserNameIndex");
            DropIndex("Finances.Receivable", new[] { "SaleId" });
            DropIndex("Finances.SalePayment", new[] { "ReceivableId" });
            DropIndex("Finances.SalePayment", new[] { "SaleId" });
            DropIndex("Operative.SaleHistory", new[] { "SaleId" });
            DropIndex("Operative.SaleDetail", "IDX_InsDate");
            DropIndex("Operative.SaleDetail", new[] { "ProductId" });
            DropIndex("Operative.SaleDetail", new[] { "SaleId" });
            DropIndex("Operative.SaleCreditNote", "IDX_Sequential");
            DropIndex("Operative.SaleCreditNote", "IDX_Identifier_Active");
            DropIndex("Operative.SaleCreditNote", new[] { "SaleCreditNoteId" });
            DropIndex("Operative.Sale", "IDX_Status");
            DropIndex("Operative.Sale", "IDX_UserId");
            DropIndex("Operative.Sale", "IDX_TransactionDate");
            DropIndex("Operative.Sale", "IDX_BranchId");
            DropIndex("Operative.Sale", "IDX_Sequential");
            DropIndex("Operative.Sale", "IDX_Year");
            DropIndex("Operative.Sale", new[] { "ClientId" });
            DropIndex("Catalog.Client", "IDX_Code");
            DropIndex("Config.State", "IDX_Name");
            DropIndex("Config.City", "IDX_StateId");
            DropIndex("Catalog.Address", new[] { "ProviderId" });
            DropIndex("Catalog.Address", new[] { "EmployeeId" });
            DropIndex("Catalog.Address", new[] { "ClientId" });
            DropIndex("Catalog.Address", "IDX_CityId");
            DropIndex("Catalog.Provider", "IDX_Email");
            DropIndex("Catalog.Provider", "Unq_FTR");
            DropIndex("Catalog.Provider", "IDX_Name");
            DropIndex("Catalog.Provider", "IDX_Code");
            DropIndex("Catalog.Equivalence", "IDX_Code");
            DropIndex("Catalog.Equivalence", "IDX_Provider_Product");
            DropIndex("Config.CarModel", "IDX_CarMakeId");
            DropIndex("Config.CarYear", "IDX_CarModelId");
            DropIndex("Catalog.Compatibility", new[] { "ProductId" });
            DropIndex("Catalog.Compatibility", new[] { "CarYearId" });
            DropIndex("Config.PartSystem", "IDX_Name");
            DropIndex("Config.SystemCategory", new[] { "CategoryId" });
            DropIndex("Config.SystemCategory", new[] { "PartSystemId" });
            DropIndex("Config.Category", "IDX_SatCode");
            DropIndex("Config.Category", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_Lock");
            DropIndex("Catalog.Product", "IDX_Ident_TradeMark");
            DropIndex("Catalog.Product", new[] { "PartSystemId" });
            DropIndex("Catalog.Product", new[] { "CategoryId" });
            DropIndex("Operative.TrackingItem", new[] { "ProductId" });
            DropIndex("Operative.InventoryItem", new[] { "TrackingItemId" });
            DropIndex("Operative.InventoryItem", new[] { "BranchId", "ProductId" });
            DropIndex("Operative.BranchProduct", "IDX_UserLock");
            DropIndex("Operative.BranchProduct", "IDX_LockDate");
            DropIndex("Operative.BranchProduct", new[] { "ProductId" });
            DropIndex("Operative.BranchProduct", new[] { "BranchId" });
            DropTable("Config.Variable");
            DropTable("Catalog.TempExternalProduct");
            DropTable("Operative.ShoppingCart");
            DropTable("Security.AspNetRoles");
            DropTable("Operative.CreditNoteHistory");
            DropTable("Operative.Budget");
            DropTable("Operative.BudgetDetail");
            DropTable("Operative.WithdrawalCause");
            DropTable("Operative.CashDetail");
            DropTable("Operative.CashRegister");
            DropTable("Catalog.PackageDetail");
            DropTable("Catalog.ProductImage");
            DropTable("Catalog.ShipMethod");
            DropTable("Purchasing.PurchaseStatus");
            DropTable("Finances.PurchasePayment");
            DropTable("Finances.PurchaseDiscount");
            DropTable("Purchasing.PurchaseDetail");
            DropTable("Purchasing.Purchase");
            DropTable("Purchasing.PurchaseOrderHistory");
            DropTable("Operative.StockMovement");
            DropTable("Purchasing.PurchaseOrderDetail");
            DropTable("Purchasing.PurchaseOrder");
            DropTable("Purchasing.PurchaseType");
            DropTable("Purchasing.PurchaseItem");
            DropTable("Catalog.ExternalProduct");
            DropTable("Config.UserBranch");
            DropTable("Security.AspNetUserRoles");
            DropTable("Security.AspNetUserLogins");
            DropTable("Config.JobPosition");
            DropTable("Catalog.Employee");
            DropTable("Security.AspNetUserClaims");
            DropTable("Security.AspNetUsers");
            DropTable("Finances.Receivable");
            DropTable("Finances.SalePayment");
            DropTable("Operative.SaleHistory");
            DropTable("Operative.SaleDetail");
            DropTable("Operative.SaleCreditNote");
            DropTable("Operative.Sale");
            DropTable("Catalog.Client");
            DropTable("Config.State");
            DropTable("Config.City");
            DropTable("Catalog.Address");
            DropTable("Catalog.Provider");
            DropTable("Catalog.Equivalence");
            DropTable("Config.CarMake");
            DropTable("Config.CarModel");
            DropTable("Config.CarYear");
            DropTable("Catalog.Compatibility");
            DropTable("Config.PartSystem");
            DropTable("Config.SystemCategory");
            DropTable("Config.Category");
            DropTable("Catalog.Product");
            DropTable("Operative.TrackingItem");
            DropTable("Operative.InventoryItem");
            DropTable("Operative.BranchProduct");
            DropTable("Config.Branch");
        }
    }
}
