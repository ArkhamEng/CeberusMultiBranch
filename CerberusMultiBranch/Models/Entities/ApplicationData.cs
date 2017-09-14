using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using System.Data.Entity;

namespace CerberusMultiBranch.Models.Entities
{
    public class ApplicationData: DbContext
    {
        #region Config

        public DbSet<Category> Categories { get; set; }

        public DbSet<CarMake> CarMakes { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<CarYear> CarYears { get; set; }

        public DbSet<Branch> Branches { get; set; }
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

        #region Inventory
        public DbSet<ProductInventory> ProductInventories { get; set; }
        #endregion

       

        

        public ApplicationData(): 
            base("DefaultConnection")
        {
        }
    }
}