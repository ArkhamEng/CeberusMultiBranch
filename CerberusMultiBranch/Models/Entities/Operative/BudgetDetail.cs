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
    [Table("BudgetDetail", Schema = "Operative")]
    public class BudgetDetail
    {
        [Column(Order =0),Key,ForeignKey("Budget")]
        public int BudgetId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Product")]
        public int ProductId { get; set; }

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

    
        #region Navitation Properties
   
        public virtual Product Product { get; set; }

        public virtual Budget Budget { get; set; }

        #endregion
    }
}