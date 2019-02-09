using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Purchasing
{
    [Table("PurchaseItem", Schema = "Purchasing")]
    public class PurchaseItem
    {
        [MaxLength(128)]
        [Column(Order = 0), Key, ForeignKey("User")]
        public string UserId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Column(Order = 2), Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        public int ProviderId { get; set; }

        public double Price { get; set; }

        public double Discount { get; set; }

        public double Quantity { get; set; }

        public double TotalLine { get; set; }

        #region Navigation Properties
        public virtual Provider Provider { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Product Product { get; set; }

        public PurchaseType PurchaseType { get; set; }

        public ApplicationUser User { get; set; }

        
        #endregion
    }
}