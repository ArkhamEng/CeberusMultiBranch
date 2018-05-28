using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("ShoppingCart", Schema = "Operative")]
    public class ShoppingCart
    {
        [MaxLength(128)]
        [Column(Order = 0), Key, ForeignKey("User")]
        public string UserId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Column(Order = 2), Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "IVA")]
        [DataType(DataType.Currency)]
        public double TaxPercentage { get; set; }

        [Required]
        [Display(Name = "Monto IVA")]
        [DataType(DataType.Currency)]
        public double TaxAmount { get; set; }

        [Required]
        [Display(Name = "Precio con IVA")]
        [DataType(DataType.Currency)]
        public double TaxedPrice { get; set; }


        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        [Display(Name = "Importe")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Display(Name = "Importe con IVA")]
        [DataType(DataType.Currency)]
        public double TaxedAmount { get; set; }

        public DateTime InsDate { get; set; }

        public int? BudgetId { get; set; }

        [NotMapped]
        public double InStock { get; set; }

        public bool CanSell { get { return !(Quantity > InStock); } }

        public string RowClass
        {
            get
            {
                if (!CanSell)
                    return "danger";
                else
                    return "default";
            }
        }

        public string Image
        {
            get
            {
                if (this.Product != null && this.Product.Images.Count > Cons.Zero)
                    return this.Product.Images.FirstOrDefault().Path;
                else
                    return "/Content/Images/sinimagen.jpg";
            }
        }


        #region Navitation Properties
        public virtual ApplicationUser User { get; set; }

        public virtual Product Product { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Client Client { get; set; }
        #endregion
    }
}