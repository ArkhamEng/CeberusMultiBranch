using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("TransactionDetail", Schema = "Operative")]
    public class TransactionDetail
    {
        [Column(Order = 0), Key, ForeignKey("Transaction")]
        public int TransactionId { get; set; }

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

        [DataType(DataType.Currency)]
        public double Commission { get; set; }

        public int SortOrder { get; set; }

        public int? ParentId { get; set; }

        public virtual Product Product { get; set; }

        public virtual Transaction Transaction { get; set; }

        public virtual Sale Sale { get; set; }

        public virtual Purchase  Purchase { get; set; }

    }
}