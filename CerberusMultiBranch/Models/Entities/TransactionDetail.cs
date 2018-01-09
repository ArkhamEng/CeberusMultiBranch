using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities
{
    public class TransactionDetail
    {
        [Column(Order = 1), Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        [Display(Name = "Importe")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }


        public virtual Product Product { get; set; }
    }
}