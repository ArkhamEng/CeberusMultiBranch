using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("TransactionDetail", Schema = "Operative")]
    public class TransactionDetail
    {
        public int TransactionDetailId { get; set; }

        [Index("IDX_TransactionId", IsUnique = false)]
        public int TransactionId { get; set; }

        [Index("IDX_ProductId", IsUnique = false)]
        public int ProductId { get; set; }

        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public double Price { get; set; }

        [Display(Name ="Importe")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        public virtual Transaction Transaction { get; set; }

        public virtual Product Product { get; set; }

    }
}