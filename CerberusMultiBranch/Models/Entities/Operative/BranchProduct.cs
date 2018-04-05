using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("BranchProduct", Schema = "Operative")]
    public class BranchProduct
    {
        [Column(Order = 0), Key, ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        public double Stock { get; set; }

        public double LastStock { get; set; }

        public double Reserved { get; set; }


        [Display(Name = "Precio de Compra")]
        [DataType(DataType.Currency)]
        public double BuyPrice { get; set; }

        [Display(Name = "% Mostrador")]
        [Required]
        public int StorePercentage { get; set; }

        [Display(Name = "% Distribuidor")]
        [Required]
        public int DealerPercentage { get; set; }

        [Display(Name = "% Mayorista")]
        [Required]
        public int WholesalerPercentage { get; set; }

        [Display(Name = "Precio Mostrador")]
        [DataType(DataType.Currency)]
        [Required]
        public double StorePrice { get; set; }

        [Display(Name = "Precio Mayorista")]
        [DataType(DataType.Currency)]
        [Required]
        public double WholesalerPrice { get; set; }

        [Display(Name = "Precio Distribuidor")]
        [DataType(DataType.Currency)]
        [Required]
        public double DealerPrice { get; set; }

        [Display(Name = "Fila")]
        [MaxLength(30)]
        public string Row { get; set; }

        [Display(Name = "Anaquel")]
        [MaxLength(30)]
        public string Ledge { get; set; }

        public DateTime UpdDate { get; set; }

        public bool StockLocked { get; set; }

        [Index("IDX_LockDate")]
        public DateTime? LockDate { get; set; }

        [Index("IDX_UserLock")]
        [MaxLength(30)]
        public string UserLock { get; set; }

        [MaxLength(100)]
        public string UpdUser { get; set; }

        #region Navigation Properties
        public virtual Branch Branch { get; set; }

        public virtual Product Product { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; }
        #endregion
    }
}