using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Config;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities
{
    public class ApplicationData: DbContext
    {
        #region Config

        public DbSet<Category> Categories { get; set; }

        public DbSet<CarMake> CarMakes { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<CarYear> CarYears { get; set; }

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

        #endregion

        #region Inventory


        #endregion

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ProductInBranch> ProductsIsBranch { get; set; }

        public ApplicationData(): 
            base("DefaultConnection")
        {
        }
    }
}