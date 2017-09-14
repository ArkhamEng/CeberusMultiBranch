using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("ProductInventory", Schema = "Operative")]
    public class ProductInventory
    {
        public int ProductInventoryId { get; set; }

        public int ProductId { get; set; }

        public int BranchId { get; set; }

        public double Stock { get; set; }

        #region Navigation Properties
        public virtual  Product Product { get; set; }

        public virtual Branch Branch { get; set; }
        #endregion
    }
}