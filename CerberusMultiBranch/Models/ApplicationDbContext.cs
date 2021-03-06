﻿using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Finances;
using CerberusMultiBranch.Models.Entities.Inventory;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.Entities.Purchasing;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CerberusMultiBranch.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Config
        public DbSet<Variable> Variables { get; set; }

        public DbSet<JobPosition> JobPositions { get; set; }

        public DbSet<WithdrawalCause> WithdrawalCauses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CarMake> CarMakes { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<CarYear> CarYears { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<PartSystem> Systems { get; set; }

        public DbSet<SystemCategory> SystemCategories { get; set; }

        public DbSet<UserBranch> UserBranches { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }
        #endregion

        #region Catalogs

        public DbSet<Product> Products { get; set; }

        public DbSet<PackageDetail> PackageDetails { get; set; }

        public DbSet<ExternalProduct> ExternalProducts { get; set; }

        public DbSet<TempExternalProduct> TempExternalProducts { get; set; }

        public DbSet<Equivalence> Equivalences { get; set; }

        public DbSet<Compatibility> Compatibilites { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ShipMethod> ShipMethodes { get; set; }

        #endregion

        #region Operative

        public DbSet<Offer> Offers { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        public DbSet<SaleCreditNote> SaleCreditNotes { get; set; }

        public DbSet<CreditNoteHistory> CreditNoteHistories { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchaseDiscount> PurchaseDiscount { get; set; }

        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }

        public DbSet<SaleHistory> SaleHistories { get; set; }

        public DbSet<CashRegister> CashRegisters { get; set; }

        public DbSet<CashDetail> CashDetails { get; set; }
        
        public DbSet<BranchProduct> BranchProducts { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<BudgetDetail> BudgetDetails { get; set; }

        public DbSet<TrackingItem> TrackingItems { get; set; }

        public DbSet<ItemLocation> ItemLocations { get; set; }

        #endregion

        #region Finances

        public DbSet<SalePayment> SalePayments { get; set; }

        public DbSet<PurchasePayment> PurchasePayments { get; set; }

        #endregion

        #region Purchasing

        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<PurchaseOrderHistory> PurchaseOrderHistories { get; set; }

        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public DbSet<PurchaseType> PurchaseTypes { get; set; }

        public DbSet<PurchaseStatus> PurchaseStatuses { get; set; }

        #endregion

        #region Inventory

        public DbSet<StockCount> StockCounts { get; set; }

        public DbSet<StockCountDetail> StockCountDetails { get; set; }

        #endregion

        public ApplicationDbContext() : base("LocalConnection", throwIfV1Schema: false)
        { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Security");

            //modelBuilder.Entity<PackageDetail>()
            //       .HasRequired(m => m.Package)
            //       .WithMany(t => t.Packages)
            //       .HasForeignKey(m => m.PackageId)
            //       .WillCascadeOnDelete(false);

            //modelBuilder.Entity<PackageDetail>()
            //            .HasRequired(m => m.Detail)
            //            .WithMany(t => t.PackageDetails)
            //            .HasForeignKey(m => m.PackageDetailId)
            //            .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

        }

    }
}