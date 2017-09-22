using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using CerberusMultiBranch.Models.Entities.Catalog;
using System.ComponentModel.DataAnnotations;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Operative;
using System.Collections.Generic;

namespace CerberusMultiBranch.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationUser : IdentityUser
    {
        public byte[] Picture { get; set; }

        public string PictureType { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<Employee> Employees { get; set; }

        [NotMapped]
        public byte[] ClearImage { get { return Support.GzipWrapper.Decompress(this.Picture); } }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }

  

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Config

        public DbSet<Category> Categories { get; set; }

        public DbSet<CarMake> CarMakes { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<CarYear> CarYears { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<EmployeeBranch> EmployeeBranches { get; set; }
        #endregion

        #region Common

        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }

        #endregion

        #region Catalogs

        public DbSet<Product> Products { get; set; }

        public DbSet<Compatibility> Compatibilites { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        #endregion

        #region Operative

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<TransactionDetail> TransactionDetailes { get; set; }
        #endregion



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Security");

            base.OnModelCreating(modelBuilder);
            
        }

        
    }
}